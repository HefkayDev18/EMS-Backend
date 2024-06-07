using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.Entities
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public SystemUsers? User { get; set; }

        public int? RoleId { get; set; }

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
        [Phone]
        [StringLength(15)]
        public required string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public required string Department { get; set; }

        [Required]
        [StringLength(100)]
        public required string Gender { get; set; }

        [Required]
        [StringLength(100)]
        public required string Position { get; set; }

        [Required]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }


    }
}
