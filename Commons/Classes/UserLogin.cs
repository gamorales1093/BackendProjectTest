using System.ComponentModel.DataAnnotations;

namespace Commons.Classes
{
    public class UserLogin
    {
        [Required(ErrorMessage = "The field {0} is required")]
        [EmailAddress(ErrorMessage = "The field {0} must be a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public string Password { get; set; }
    }
}
