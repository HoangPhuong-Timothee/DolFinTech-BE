using System.ComponentModel.DataAnnotations;

namespace API.DTOs.User
{
    public class RegisterAccountRequest
    {
        [Required]
        public string? Usename { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}