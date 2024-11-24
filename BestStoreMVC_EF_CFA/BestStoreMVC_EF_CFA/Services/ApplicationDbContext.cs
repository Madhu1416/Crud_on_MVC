using BestStoreMVC_EF_CFA.Models;
using Microsoft.EntityFrameworkCore;

namespace BestStoreMVC_EF_CFA.Services
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Product> Products { set; get; }

    }
}
