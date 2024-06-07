using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.Entities
{
    public class Faculty
    {
        [Key]
        public int FacultyId { get; set; }
        public required string FacultyName { get; set; }
    }
}
