namespace EmployeeManagementSystem.Models.ViewModels.CoreFeaturesVMs
{
    public class LodgeMedRecordVM
    {
        public int EmployeeId { get; set; }
        public required string Diagnosis { get; set; }
        public required string Prescription { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public required string DoctorName { get; set; }
        public string? Comments { get; set; }
    }

    public class AddMedRecordVM
    {
        public required string Diagnosis { get; set; }
        public required string Prescription { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public required string DoctorName { get; set; }
        public string? Comments { get; set; }
    }
}
