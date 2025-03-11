using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Proteger todos los endpoints del controlador
public class UsuariosController : ControllerBase
{
    private readonly BancoContext _context;

    public UsuariosController (BancoContext context) {
        _context = context;
    }

    //Obtener todos los usuarios
    //GET: api/Usuarios
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        => await _context.Usuarios.Include(u => u.Cuentas).ToListAsync();

    // Obtener usuario por ID
    // GET: api/Usuarios/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Cuentas)
            .FirstOrDefaultAsync(u => u.ID == id);

        return usuario == null ? NotFound() : Ok(usuario);
    }

    // Crear usuario
    // POST: api/Usuarios
    [HttpPost]
    public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
    {
        // Validar que el email no exista
        if (await _context.Personas.AnyAsync(p => p.Email == usuario.Email))
            return BadRequest("El correo ya está registrado");

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.ID }, usuario);
    }

    // Actualizar usuario
    // PUT: api/Usuarios/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUsuario(int id, Usuario usuario) {
        if (id != usuario.ID) {
            return BadRequest("ID del usuario no coincide");
        }

        //Validar que el email no esté en uso por otro usuario 
        if (await _context.Personas.AnyAsync(p => p.Email == usuario.Email && p.ID != id))
            return BadRequest("El correo ya está registrado");

        _context.Entry(usuario).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        } catch (DbUpdateConcurrencyException) {
            if(!UsuarioExists(id))
                return NotFound();
            throw;
        }

        // Mensaje de éxito con el usuario actualizado
        return Ok(new 
        { 
            mensaje = "Usuario actualizado correctamente",
            usuarioActualizado = usuario 
        });
    }

    //Eliminar Usuario
    // DELETE: api/Usuarios/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return NotFound();

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Usuario eliminado correctamente"});
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetUsuarioActual()
    {
        // Obtener el ID del usuario autenticado desde el token (si estás usando JWT)
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("ID de usuario no válido.");
        }

        // Buscar el usuario en la base de datos
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.ID == int.Parse(userId));

        if (usuario == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        return Ok(usuario);
    }

    private bool UsuarioExists(int id) => _context.Usuarios.Any(e => e.ID == id);

}