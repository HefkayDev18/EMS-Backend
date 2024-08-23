using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.Entities
{
    public class Credentials
    {
        [Key]
        public int UploadId { get; set; }
        public int EmployeeId { get; set; }
        public Employee? EmployeeCred { get; set; }
        public required string DocumentType { get; set; }
        public string? Description { get; set; }
        public required string FilePath { get; set; } 
        public DateTime UploadedDate { get; set; }
    }
}
