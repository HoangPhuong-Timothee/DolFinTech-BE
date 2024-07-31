using API.Data.SeedData;
using API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DolFinTechDbContext : IdentityDbContext<AppUser>
    {
        public DolFinTechDbContext(DbContextOptions<DolFinTechDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // new DolFinTechDbInitialize(modelBuilder).SeedData();
            new DolFinTechDbInitialize(modelBuilder).ConfigData();
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
    }
}