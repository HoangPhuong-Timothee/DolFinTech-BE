using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data.SeedData
{
    public class DolFinTechDbInitialize
    {
        private readonly ModelBuilder _modelBuilder;
        public DolFinTechDbInitialize(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void SeedData()
        {
            //Stock
            // _modelBuilder.Entity<Stock>().HasData(
            //     new Stock { Id = 21, Symbol = "TSLA", CompanyName = "Tesla", Purchase = 100M, LastDiv = 2M, Industry = "Automotive", MarketCap = 234234234 },
            //     new Stock { Id = 22, Symbol = "MSFT", CompanyName = "Microsoft", Purchase = 100M, LastDiv = 1.2M, Industry = "Technology", MarketCap = 25443432 },
            //     new Stock { Id = 23, Symbol = "VTI", CompanyName = "Vanguard Total Index", Purchase = 100M, LastDiv = 2.1m, Industry = "Index Fund", MarketCap = 2555527 },
            //     new Stock { Id = 24, Symbol = "PLTR", CompanyName = "Plantir", Purchase = 23M, LastDiv = 0, Industry = "Technology", MarketCap = 1243000 }   
            // );
            
            //Comment
            // _modelBuilder.Entity<Comment>().HasData(
            //     new Comment { Id = 2, StockId = 22, Title = "Great investment opportunity!", Content = "I will all in Microsoft forever!!!", CreatedAt = DateTime.Now },
            //     new Comment { Id = 4, StockId = 22, Title = "Not good for investment!", Content = "Never trust Bill Gates, guys!!!", CreatedAt = DateTime.Now }
            // );
        }

        public void ConfigData()
        {
            //Identity
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Name = "User", NormalizedName = "USER" }
            };
            _modelBuilder.Entity<IdentityRole>().HasData(roles);

            //Favorite
            _modelBuilder.Entity<Favorite>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));
            _modelBuilder.Entity<Favorite>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Favorites)
                .HasForeignKey(p => p.AppUserId);
            _modelBuilder.Entity<Favorite>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.Favorites)
                .HasForeignKey(p => p.StockId);
        }
    }
}