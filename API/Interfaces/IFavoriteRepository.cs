using API.Models;

namespace API.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<List<Stock>> GetUserStockAsync(AppUser appUser);
        Task<Favorite> AddToFavoritesAsync(Favorite favorite);
        Task<Favorite> RemoveFromFavoritesAsync(AppUser appUser, string symbol);
    }
}