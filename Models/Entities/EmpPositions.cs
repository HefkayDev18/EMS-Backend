using EmployeeManagementSystem.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.Entities
{
    public class EmpPositions
    {
        [Key]
        public int PositionId { get; set; }
        public int EmployeeId { get; set; }
        public required string Position { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime? DateEnded { get; set; } 
        public int DepartmentId { get; set; } 
        public required string DepartmentName { get; set; }

    }
}


