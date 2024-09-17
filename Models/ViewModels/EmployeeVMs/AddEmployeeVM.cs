using EmployeeManagementSystem.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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


    public class ViewEmpVM
    {
        public int EmployeeId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public string? Gender { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
        public decimal Salary { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public DateTime DateCreated { get; set; }
        public bool HasEmploymentHistory { get; set; }
        public DateTime DateOfBirth { get; set; }

        [JsonIgnore]
        public virtual ICollection<EmpHistory> EmploymentHistories { get; set; } = [];

        [JsonIgnore]
        public virtual ICollection<EmpAppraisal> Appraisals { get; set; } = [];

        [JsonIgnore]
        public virtual ICollection<EmpMedRecords> EmpMedRecords { get; set; } = [];
    }
}
