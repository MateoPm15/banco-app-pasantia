using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Movimiento {
    [Key]
    public int ID { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    [StringLength(20)]
    public string TipoMovimiento { get; set; } = "Crédito";

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Valor { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Saldo { get; set; }

    // Clave foránea (relación con Cuenta)
    [Required]
    public string CuentaId { get; set; } = string.Empty;

    // Propiedad de navegación
    [ForeignKey("CuentaId")]
    [JsonIgnore] // Evita ciclos de referencia
    public Cuenta? Cuenta { get; set; }
}