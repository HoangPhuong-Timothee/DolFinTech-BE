using API.Data;
using API.DTOs.Stock;
using API.Helpers;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class StockRepository : IStockRepository //public class StockRepository : GenericRepository<Stock>, IStockRepository
    {
        private readonly DolFinTechDbContext _context;
        public StockRepository(DolFinTechDbContext context) //public StockRepository(DolFinTechDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
           var stocks = _context.Stocks.Include(s => s.Comments).ThenInclude(s => s.AppUser).AsQueryable();
           if (!string.IsNullOrWhiteSpace(query.CompanyName))
           {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
           }
           if (!string.IsNullOrWhiteSpace(query.Symbol))
           {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
           }
           if (!string.IsNullOrWhiteSpace(query.OrderBy))
           {
                if (query.OrderBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
                if (query.OrderBy.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                }
                if (query.OrderBy.Equals("Purchase", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Purchase) : stocks.OrderBy(s => s.Purchase);
                }
                if (query.OrderBy.Equals("LastDiv", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.LastDiv) : stocks.OrderBy(s => s.LastDiv);
                }
                if (query.OrderBy.Equals("MarketCap", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.MarketCap) : stocks.OrderBy(s => s.MarketCap);
                }
           }
           var skip = (query.PageNumber - 1) * query.PageSize;
           return await stocks.Skip(skip).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(s => s.Comments).ThenInclude(s => s.AppUser).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
        }

          public async Task CreateAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequest request)
        {
            var existedStock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if(existedStock == null)
            {
                return null;
            }
            existedStock.Symbol = request.Symbol;
            existedStock.CompanyName = request.CompanyName;
            existedStock.Purchase = request.Purchase;
            existedStock.LastDiv = request.LastDiv;
            existedStock.Industry = request.Industry;
            existedStock.MarketCap = request.MarketCap;
            await _context.SaveChangesAsync();
            return existedStock;
        }

         public async Task<Stock?> DeleteAsync(int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if(stock == null)
            {
                return null;    
            }
            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<bool> IsStockExist(int id)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == id);
        }
    }
}