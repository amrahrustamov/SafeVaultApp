using System.ComponentModel.DataAnnotations;

namespace SafeVaultApp.Models
{
    public class User
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [RegularExpression("Admin|User|Manager", ErrorMessage = "Invalid role.")]
        public string Role { get; set; }
    }

}
