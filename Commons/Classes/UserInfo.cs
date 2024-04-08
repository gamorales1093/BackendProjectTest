using System.ComponentModel.DataAnnotations;

namespace Commons.Classes
{
    public class UserInfo
    {
        [Required(ErrorMessage = "The field {0} is required")]
        [EmailAddress(ErrorMessage = "The field {0} must be a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}