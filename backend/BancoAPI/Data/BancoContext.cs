using Microsoft.EntityFrameworkCore;

public class BancoContext : DbContext {
    public BancoContext(DbContextOptions<BancoContext> options) : base(options) {}

    // Definición de las entidades en la base de datos
    public DbSet<Persona> Personas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cuenta> Cuentas { get; set; }
    public DbSet<Movimiento> Movimientos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // Configuración de relaciones

        // Relación Usuario → Cuenta (1 a muchos)
        modelBuilder.Entity<Cuenta>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.Cuentas)
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relación Cuenta → Movimiento (1 a muchos)
        modelBuilder.Entity<Movimiento>()
            .HasOne(m => m.Cuenta)
            .WithMany(c => c.Movimientos)
            .HasForeignKey(m => m.CuentaId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de propiedades requeridas
        modelBuilder.Entity<Usuario>()
            .Property(u => u.Contrasenia)
            .IsRequired();

        modelBuilder.Entity<Movimiento>()
            .Property(m => m.TipoMovimiento)
            .IsRequired();
    }
}