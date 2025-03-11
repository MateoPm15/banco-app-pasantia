using System.ComponentModel.DataAnnotations;

public class Persona {
    [Key]
    public int ID { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    public string Direccion { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;
}