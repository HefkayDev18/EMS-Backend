namespace EmployeeManagementSystem.Models.ViewModels.CoreFeaturesVMs
{
    public class AddAppraisalVM
    {
        public DateTime? AppDateSubmitted { get; set; }
        public required string Status { get; set; }
        public int PublicationProgress { get; set; }
        public int Teaching { get; set; }
        public int PatentConferencing { get; set; }
        public int CommunityService { get; set; }
        public int AdministrationExperience { get; set; }
        public int Communication { get; set; }
        public int Teamwork { get; set; }
        public int Leadership { get; set; }
        public int ProblemSolving { get; set; }
        public int Punctuality { get; set; }
        public int Adaptability { get; set; }
        public int OverallSatisfaction { get; set; }
        public required string Comments { get; set; }
    }

    public class ReviewAppraisalVM
    {
        public DateTime? ReviewedAt { get; set; }
        public required string Status { get; set; }
        public string? ManagerComment { get; set; }
    }
}
