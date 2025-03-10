using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Proteger todos los endpoints del controlador

public class CuentasController : ControllerBase {
    private readonly BancoContext _context;

    public CuentasController(BancoContext context) {
        _context = context;
    }

    //Obtener todas las cuentas
    //GET: api/Cuentas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cuenta>>> GetCuentas()
        => await _context.Cuentas.Include(c => c.Usuario).ToListAsync();

    //Crear una nueva cuenta
    // POST: api/Cuentas
    public async Task<ActionResult<Cuenta>> PostCuenta(Cuenta cuenta) {
        //Validar que el usuario exista
        var usuario = await _context.Usuarios.FindAsync(cuenta.UsuarioId);
        if(usuario == null) return BadRequest("Usuario no encontrado");

        //Generar número de cuenta único
        cuenta.NumeroCuenta = DateTime.Now.Ticks.ToString();

        _context.Cuentas.Add(cuenta);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCuenta), new { numeroCuenta = cuenta.NumeroCuenta}, cuenta);
    }

    //Obtener cuenta por número
    // GET: api/Cuentas/123456789
    [HttpGet("{numeroCuenta}")]
    public async Task<ActionResult<Cuenta>> GetCuenta(string numeroCuenta) {
        var cuenta = await _context.Cuentas
            .Include(c => c.Usuario)
            .Include(c => c.Movimientos)
            .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);

        return cuenta == null ? NotFound() : Ok(cuenta);
    }

    //Actualizar cuenta
    // PUT: api/Cuentas/123456789
    [HttpPut("{numeroCuenta}")]
    public async Task<IActionResult> PutCuenta(string numeroCuenta, Cuenta cuenta) {
        if (numeroCuenta != cuenta.NumeroCuenta) {
            return BadRequest("Número de cuenta no coincide");
        }

        _context.Entry(cuenta).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            if(!CuentaExists(numeroCuenta))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    //Eliminar cuenta
    [HttpDelete("{numeroCuenta}")]
    public async Task<IActionResult> DeleteCuenta(string numeroCuenta) {
        var cuenta = await _context.Cuentas.FindAsync(numeroCuenta);
        if (cuenta == null) return NotFound();

        _context.Cuentas.Remove(cuenta);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CuentaExists(string numeroCuenta) => _context.Cuentas.Any(e => e.NumeroCuenta == numeroCuenta);
}
