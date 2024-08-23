using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.ViewModels.EmployeeVMs
{
    public class AddEmployeeVM
    {
        [Required]
        [StringLength(100)]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public required string Email { get; set; }

        [Required]
        [StringLength(100)]
        public required string Department { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public required string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public required string Gender { get; set; }

        [Required]
        [StringLength(100)]
        public required string Position { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public bool HasEmploymentHistory { get; set; } = false;
    }
}
