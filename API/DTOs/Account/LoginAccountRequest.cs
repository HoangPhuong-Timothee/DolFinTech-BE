using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User
{
    public class LoginAccountRequest
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}