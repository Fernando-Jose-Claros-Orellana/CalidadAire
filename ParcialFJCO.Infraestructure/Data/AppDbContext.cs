using Microsoft.EntityFrameworkCore;
using ParcialFJCO.Domain.Entities;

namespace ParcialFJCO.Infraestructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<SensorCalidadAire> SensoresCalidadAire { get; set; }
        public DbSet<LecturaAire> LecturasAire { get; set; }
        public DbSet<AlertaAire> AlertasAire { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(u => u.Username)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(u => u.Username)
                    .IsUnique();

                entity.Property(u => u.Role)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("User");
            });

            modelBuilder.Entity<Usuario>().HasData(new Usuario
            {
                Id = 1,
                Username = "usuarioAdmin",
                PasswordHash = "$2a$11$NfCjQSpWiylUK5OzOypVteoQ4kBdr9mX7b6Gs/x1Oj9101PjF1c32",
                Role = "Admin"
            });

            modelBuilder.Entity<SensorCalidadAire>(entity =>
            {
                entity.Property(s => s.Ubicacion)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(s => s.TipoGas)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(s => s.Estado)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValue("Activo");
            });

            modelBuilder.Entity<LecturaAire>(entity =>
            {
                entity.Property(l => l.PM2_5)
                    .HasColumnType("decimal(18,2)");

                entity.Property(l => l.PM10)
                    .HasColumnType("decimal(18,2)");

                entity.Property(l => l.CO2)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(l => l.Sensor)
                    .WithMany(s => s.Lecturas)
                    .HasForeignKey(l => l.SensorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AlertaAire>(entity =>
            {
                entity.Property(a => a.Nivel)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(a => a.Mensaje)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(a => a.Sensor)
                    .WithMany(s => s.Alertas)
                    .HasForeignKey(a => a.SensorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SensorCalidadAire>().HasData(
                new SensorCalidadAire
                {
                    Id = 1,
                    Ubicacion = "Planta 1 - Zona A",
                    TipoGas = "PM2.5/PM10/CO2",
                    Estado = "Activo"
                },
                new SensorCalidadAire
                {
                    Id = 2,
                    Ubicacion = "Planta 1 - Zona B",
                    TipoGas = "PM2.5/CO2",
                    Estado = "Activo"
                });

            modelBuilder.Entity<LecturaAire>().HasData(
                new LecturaAire
                {
                    Id = 1,
                    SensorId = 1,
                    PM2_5 = 12,
                    PM10 = 20,
                    CO2 = 600,
                    FechaHora = new DateTime(2026, 5, 19, 3, 40, 0, DateTimeKind.Utc)
                },
                new LecturaAire
                {
                    Id = 2,
                    SensorId = 1,
                    PM2_5 = 80,
                    PM10 = 120,
                    CO2 = 900,
                    FechaHora = new DateTime(2026, 5, 19, 3, 45, 0, DateTimeKind.Utc)
                },
                new LecturaAire
                {
                    Id = 3,
                    SensorId = 2,
                    PM2_5 = 40,
                    PM10 = 60,
                    CO2 = 6001,
                    FechaHora = new DateTime(2026, 5, 19, 3, 50, 0, DateTimeKind.Utc)
                });

            modelBuilder.Entity<AlertaAire>().HasData(
                new AlertaAire
                {
                    Id = 1,
                    SensorId = 1,
                    Nivel = "Moderada",
                    Mensaje = "La calidad del aire es poco saludable para grupos sensibles (niños, adultos mayores, personas con enfermedades respiratorias).",
                    FechaHora = new DateTime(2026, 5, 19, 3, 45, 0, DateTimeKind.Utc)
                },
                new AlertaAire
                {
                    Id = 2,
                    SensorId = 2,
                    Nivel = "Extrema",
                    Mensaje = "Nivel de contaminación extremadamente alto. Riesgo severo para la salud.",
                    FechaHora = new DateTime(2026, 5, 19, 3, 50, 0, DateTimeKind.Utc)
                });
        }
    }
}
