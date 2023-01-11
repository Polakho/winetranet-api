using System.ComponentModel.DataAnnotations;

namespace winetranet_api.Entities
{
    public class Login
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
