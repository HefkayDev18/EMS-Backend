using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models.Entities
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public required string DepartmentName { get; set; }
        public required string Description { get; set; }
        public required string HeadOfDepartment { get; set; }
        public required string FacultyName { get; set; }

    }
}
