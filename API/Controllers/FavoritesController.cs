using API.Extensions;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IFavoriteRepository _favoriteRepo;
        public FavoritesController(IStockRepository stockRepo, UserManager<AppUser> userManager, IFavoriteRepository favoriteRepo)
        {
            _stockRepo = stockRepo;
            _userManager = userManager;
            _favoriteRepo = favoriteRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserFavoritesAsync()
        {
            try
            {
                var userName = User.GetUserName();
                var appUser = await _userManager.FindByNameAsync(userName);
                var userFavorites = await _favoriteRepo.GetUserStockAsync(appUser);
                return Ok(userFavorites);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddToUserFavoriteAsync(string symbol)
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _userManager.FindByNameAsync(userName);
                var stock = await _stockRepo.GetBySymbolAsync(symbol);
                if (stock == null) return BadRequest("Stock not found.");
                var userFavorite = await _favoriteRepo.GetUserStockAsync(user);
                if (userFavorite.Any(s => s.Symbol.ToLower() == symbol.ToLower()))
                {
                    return BadRequest("Stock already in your favorites list.");
                }
                var favorite = new Favorite
                {
                    StockId = stock.Id,
                    AppUserId = user.Id
                };
                await _favoriteRepo.AddToFavoritesAsync(favorite);
                if (favorite == null)
                {
                    return BadRequest("Cannot add stock to your favorites.");
                }
                else
                {
                    return StatusCode(201, "Stock has been added to your favorites.");
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteFromUserFavoriteAsync(string symbol)
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _userManager.FindByNameAsync(userName);
                var favorite = await _favoriteRepo.GetUserStockAsync(user);
                var filteredStock = favorite.Where(f => f.Symbol.ToLower() == symbol.ToLower());
                if (filteredStock.Count() == 1)
                {
                    await _favoriteRepo.RemoveFromFavoritesAsync(user, symbol);
                }
                else
                {
                    return BadRequest("Stock is not in your favorites.");
                }
                return Ok("Stock has been removed from favorites.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}