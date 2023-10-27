using Microsoft.EntityFrameworkCore;

namespace BackDestiCode.Data.Context
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) :base(options)
        {
            
        }
    }
}
