﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BancoAPI.Migrations
{
    [DbContext(typeof(BancoContext))]
    partial class BancoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Cuenta", b =>
                {
                    b.Property<string>("NumeroCuenta")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.Property<decimal>("Saldo")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("TipoCuenta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("NumeroCuenta");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Cuentas");
                });

            modelBuilder.Entity("Movimiento", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("CuentaId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Saldo")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("TipoMovimiento")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("ID");

                    b.HasIndex("CuentaId");

                    b.ToTable("Movimientos");
                });

            modelBuilder.Entity("Persona", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Personas");

                    b.HasDiscriminator().HasValue("Persona");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Usuario", b =>
                {
                    b.HasBaseType("Persona");

                    b.Property<string>("Contrasenia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Estado")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("Usuario");
                });

            modelBuilder.Entity("Cuenta", b =>
                {
                    b.HasOne("Usuario", "Usuario")
                        .WithMany("Cuentas")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Movimiento", b =>
                {
                    b.HasOne("Cuenta", "Cuenta")
                        .WithMany("Movimientos")
                        .HasForeignKey("CuentaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cuenta");
                });

            modelBuilder.Entity("Cuenta", b =>
                {
                    b.Navigation("Movimientos");
                });

            modelBuilder.Entity("Usuario", b =>
                {
                    b.Navigation("Cuentas");
                });
#pragma warning restore 612, 618
        }
    }
}
