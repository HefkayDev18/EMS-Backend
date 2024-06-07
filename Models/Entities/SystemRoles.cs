using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.Entities
{
    public class SystemRoles
    {
        [Key]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public required string RoleName { get; set; }
    }
}
