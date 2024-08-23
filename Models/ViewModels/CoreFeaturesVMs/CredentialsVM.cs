namespace EmployeeManagementSystem.Models.ViewModels.CoreFeaturesVMs
{
    public class CredentialsVM
    {
        public required string DocumentType { get; set; }
        public string? Description { get; set; }
        //public required IFormFile File { get; set; }
        public required IFormFileCollection Files { get; set; }
    }
}
