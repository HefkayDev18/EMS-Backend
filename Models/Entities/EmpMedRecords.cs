using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.Entities
{
    public class EmpMedRecords
    {
        [Key]
        public int MedRecordsId { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public required string Diagnosis { get; set; } 
        public required string Prescription { get; set; }
        public DateTime DateOfRecord { get; set; } 
        public DateTime? AppointmentDate { get; set; }
        public required string DoctorName { get; set; } 
        public string? Comments { get; set; } 
    }
}
