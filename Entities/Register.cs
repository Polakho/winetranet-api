using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;

namespace winetranet_api.Entities
{
    public class Register
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Email { get; set; }

        [AllowNull]

        public string Phone { get; set; }

        [Required]

        public string PhoneMobile { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, Compare(nameof(Password), ErrorMessage = "Password mismatch")]
        public string PasswordCheck { get; set; }

        [Required]
        public int? Service { get; set; }

        [Required]
        public int? Site { get; set; }
    }
}
