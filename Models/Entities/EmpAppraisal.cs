using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.Entities
{
    public class EmpAppraisal
    {
        [Key]
        public int AppraisalId { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime AppDateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? AppDateSubmitted { get; set; }
        public DateTime? ReviewedAt { get; set; }
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
        public string? Comments { get; set; }
        public string? ManagerComment { get; set; }
    }

}

