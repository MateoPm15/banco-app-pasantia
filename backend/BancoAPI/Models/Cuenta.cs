using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Cuenta {
    [Key]
    public string NumeroCuenta { get; set; } = string.Empty;

    [Required]
    public string TipoCuenta { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Saldo { get; set; }

    [Required]
    public bool Estado { get; set; }

    // Clave foránea (relación con Usuario)
    [Required]
    public int UsuarioId { get; set; }

    // Propiedad de navegación (No inicializar aquí para evitar validación)
    [JsonIgnore]
    public Usuario? Usuario { get; set; }

    // Propiedad de navegación (1 cuenta → muchos movimientos)
    public List<Movimiento> Movimientos { get; set; } = new List<Movimiento>();
}