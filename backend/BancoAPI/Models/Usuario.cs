using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Usuario : Persona {
    [Required]
    public string Contrasenia { get; set; } = string.Empty;

    [Required]
    public bool Estado { get; set; }

    // Propiedad de navegación (1 usuario → muchas cuentas)
    [JsonIgnore]
    public List<Cuenta> Cuentas { get; set; } = new List<Cuenta>();
}