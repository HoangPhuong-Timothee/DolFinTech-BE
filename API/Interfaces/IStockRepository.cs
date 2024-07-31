using API.DTOs.Stock;
using API.Helpers;
using API.Models;

namespace API.Interfaces
{
    public interface IStockRepository 
    // : IGenericRepository<Stock>
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task CreateAsync(Stock stock);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequest request);
        Task<Stock?> DeleteAsync(int id);
        Task<bool> IsStockExist(int id);
    }
}