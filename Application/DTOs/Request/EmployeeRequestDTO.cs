using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class EmployeeRequestDTO
    {
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(25)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [EmailAddress(ErrorMessage = "The field {0} must be a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(30)]
        public string Position { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(60)]
        public string Address { get; set; }
    }
}
