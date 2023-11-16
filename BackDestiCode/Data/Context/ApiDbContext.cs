using BackDestiCode.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BackDestiCode.Data.Context
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) :base(options)
        {
            
        }
         
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<DatosPersonales> DatosPersonales { get; set; }
        public DbSet<Reservaciones> Reservaciones { get; set; }
        public DbSet<Rutas> Rutas { get; set; }
        public DbSet<Vehiculos> Vehiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuarios>(dp =>
            {
                dp.HasKey(pk => pk.Id_Usuario);

                dp.HasMany(u => u.DatosPersonales)
                    .WithOne(dp => dp.Usuarios)
                    .HasForeignKey(fk => fk.Id_Usuario);

                dp.HasMany(u => u.Reservaciones)
                    .WithOne(dp => dp.Usuarios)
                    .HasForeignKey(fk => fk.Id_Usuario);

                dp.HasMany(u => u.Vehiculos)
                    .WithOne(dp => dp.Usuarios)
                    .HasForeignKey(fk => fk.Id_Usuario);

                dp.HasMany(u => u.Rutas)
                    .WithOne(dp => dp.Usuarios)
                    .HasForeignKey(fk => fk.Id_Usuario);

            });

            modelBuilder.Entity<DatosPersonales>(dp =>
            {
                dp.HasKey(pk => pk.Id_DatosPersonales);
            });

            modelBuilder.Entity<Reservaciones>(dp =>
            {
                dp.HasKey(pk => pk.Id_Reservacion);
            });

            modelBuilder.Entity<Rutas>(dp =>
            {
                dp.HasKey(pk => pk.Id_Ruta);

                dp.HasMany(u => u.Reservaciones)
                    .WithOne(dp => dp.Rutas)
                    .HasForeignKey(fk => fk.Id_Ruta);
            });

            modelBuilder.Entity<Vehiculos>(dp =>
            {
                dp.HasKey(pk => pk.Id_Unidad);

                dp.HasMany(u => u.Rutas)
                    .WithOne(dp => dp.Vehiculos)
                    .HasForeignKey(fk => fk.Id_Ruta);

            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
