using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.ViewModels.EmployeeVMs
{
    public class UpdateEmployeeVM
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
        public required string Position { get; set; }

        [Required]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }
    }


    public class UpdateEmployeeRoleVM
    {
        public int RoleId { get; set; }
        public required string RoleName { get; set; }
    }
}
