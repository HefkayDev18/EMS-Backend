using EmployeeManagementSystem.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.ViewModels.CoreFeaturesVMs
{
    public class AddEmpHistoryVM
    {
        public required DateTime DateEmployed { get; set; }
        public string? Duration { get; set; }
        public bool CurrentlyEmployed { get; set; }
        public required string Description { get; set; }
    }

    public class UpdateEmpHistoryVM
    {
        public required DateTime DateEmployed { get; set; }
        public DateTime? DateRelieved { get; set; }
        public required string Duration { get; set; }
        public bool CurrentlyEmployed { get; set; }
        public required string Description { get; set; }
    }

    public class ViewEmpHistoryVM
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime DateEmployed { get; set; }
        public DateTime? DateRelieved { get; set; }
        public required string Duration { get; set; }
        public bool CurrentlyEmployed { get; set; }
        public required string Description { get; set; }
        public List<EmpPositions> PositionsHeld { get; set; } = new List<EmpPositions>();
    }
}
