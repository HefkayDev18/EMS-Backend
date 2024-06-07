using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models.Entities
{
    public class SystemUsers
    {
        [Key]
        public int UserId { get; set; }
        public required string IdentityId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        [Required]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public SystemRoles? Role { get; set; }

        [Required]
        [StringLength(100)]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateJoined { get; set; }
    }
}
