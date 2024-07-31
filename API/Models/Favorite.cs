using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Favorites")]
    public class Favorite
    {
        public string AppUserId { get; set; }
        public int StockId { get; set; }
        public AppUser AppUser { get; set; }
        public Stock Stock { get; set; } 
    }
}