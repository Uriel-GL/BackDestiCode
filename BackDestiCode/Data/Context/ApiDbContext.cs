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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuarios>(dp =>
            {
                dp.HasKey(pk => pk.Id_Usuario);
                dp.HasMany(u => u.DatosPersonales)
                    .WithOne(dp => dp.Usuarios)
                    .HasForeignKey(fk => fk.Id_Usuario);
            });

            modelBuilder.Entity<DatosPersonales>(dp =>
            {
                dp.HasKey(pk => pk.Id_DatosPersonales);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
