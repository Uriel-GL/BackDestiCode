using BackDestiCode.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BackDestiCode.Data.Context
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) :base(options)
        {
            
        }
        public DbSet<DatosPersonales> DatosPersona { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
    }
}
