using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly DolFinTechDbContext _context;
        public FavoriteRepository(DolFinTechDbContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetUserStockAsync(AppUser appUser)
        {
            return await _context.Favorites.Where(u => u.AppUserId == appUser.Id).Select(stock => new Stock 
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                MarketCap = stock.Stock.MarketCap,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                Purchase = stock.Stock.Purchase
            }).ToListAsync();
        }

        public async Task<Favorite> AddToFavoritesAsync(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
            return favorite;
        }

        public async Task<Favorite?> RemoveFromFavoritesAsync(AppUser appUser, string symbol)
        {
            var favoriteStock = await _context.Favorites.FirstOrDefaultAsync(f => f.AppUserId == appUser.Id && f.Stock.Symbol == symbol);
            _context.Favorites.Remove(favoriteStock);
            if (favoriteStock != null)
            {
                await _context.SaveChangesAsync();
                return favoriteStock;
            }
            return null;
        }
    }
}