using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly BancoContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(BancoContext context, IConfiguration configuration)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public string? Authenticate(string email, string password)
    {
        // Validaciones de entrada
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return null;

        var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);
        
        if (usuario == null || usuario.Contrasenia != password)
            return null;

        // Configuración segura
        var secretKey = _configuration["Jwt:SecretKey"] 
            ?? throw new InvalidOperationException("Falta configuración JWT");
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("full_name", $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim("role", "user") // Agrega roles si es necesario
            }),
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
}
