using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Proteger todos los endpoints del controlador
public class MovimientosController : ControllerBase {
    private readonly BancoContext _context;

    public MovimientosController(BancoContext context) {
        _context = context;
    }

    // Obtener todos los movimientos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movimiento>>> GetMovimientos()
    {
        return await _context.Movimientos.ToListAsync();
    }

    // POST: api/Movimientos/credito
    [HttpPost("credito")]
    public async Task<ActionResult<Movimiento>> PostCredito(Movimiento movimiento) {
        // Loggear los datos recibidos
        Console.WriteLine($"Valor: {movimiento.Valor}");
        Console.WriteLine($"CuentaId: {movimiento.CuentaId}");        
        //Validar monto máximo de crédito
        if(movimiento.Valor > 3000)
            return BadRequest("El crédito no puede superar $3000");

        return await ProcesarMovimiento(movimiento, "Crédito");
    }

    // POST: api/Movimientos/debito
    [HttpPost("debito")]
    public async Task<ActionResult<Movimiento>> PostDebito(Movimiento movimiento) {
        var cuenta = await _context.Cuentas.FindAsync(movimiento.CuentaId);
        if  (cuenta == null) return NotFound("Cuenta no existe");

        //Validar saldo suficiente
        if (cuenta.Saldo < movimiento.Valor)
            return BadRequest("Saldo insuficiente");

        return await ProcesarMovimiento(movimiento, "Débito");
    }

    private async Task<ActionResult<Movimiento>> ProcesarMovimiento(Movimiento movimiento, string tipo) {
        movimiento.TipoMovimiento = tipo;
        movimiento.Fecha = DateTime.Now;

        var cuenta = await _context.Cuentas.FindAsync(movimiento.CuentaId);
        if (cuenta == null) return NotFound("Cuenta no encontrada");

        //Actualizar saldo
        cuenta.Saldo += (tipo == "Crédito") ? movimiento.Valor : -movimiento.Valor;
        movimiento.Saldo = cuenta.Saldo;

        _context.Movimientos.Add(movimiento);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMovimiento), new { id = movimiento.ID}, movimiento);
    }

    //Obtener movimiento por ID
    // GET: api/Movimientos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Movimiento>> GetMovimiento(int id) {
        var movimiento = await _context.Movimientos
            .Include(m => m.Cuenta)
            .FirstOrDefaultAsync(m => m.ID == id);

        return movimiento == null ? NotFound() : Ok(movimiento);
    }

    //Actualizar Movimiento
    // PUT: api/Movimientos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovimiento(int id, Movimiento movimiento) {
        if (id != movimiento.ID)
            return BadRequest("ID del movimiento no coincide");
        
        _context.Entry(movimiento).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            if (!MovimientoExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Movimientos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovimiento(int id)
    {
        var movimiento = await _context.Movimientos.FindAsync(id);
        if (movimiento == null) return NotFound();

        _context.Movimientos.Remove(movimiento);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Nuevo endpoint para movimientos del usuario autenticado
    [HttpGet("usuario")]
    public async Task<ActionResult<IEnumerable<Movimiento>>> GetMovimientosByUsuario()
    {
        var usuarioId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(usuarioId))
            return Unauthorized();

        var cuentasUsuario = await _context.Cuentas
            .Where(c => c.UsuarioId == int.Parse(usuarioId))
            .Select(c => c.NumeroCuenta)
            .ToListAsync();

        return await _context.Movimientos
            .Where(m => cuentasUsuario.Contains(m.CuentaId))
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
    }
    
    private bool MovimientoExists(int id) => _context.Movimientos.Any(e => e.ID == id);
}