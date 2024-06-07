using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.ViewModels
{
    public class RegisterModel
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
