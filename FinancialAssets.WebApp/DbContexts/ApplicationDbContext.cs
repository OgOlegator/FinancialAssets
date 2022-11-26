using Microsoft.EntityFrameworkCore;
using FinancialAssets.WebApp.Models;

namespace FinancialAssets.WebApp.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Asset> Assets { get; set; }

    }
}
