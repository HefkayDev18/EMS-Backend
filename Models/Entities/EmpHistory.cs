using EmployeeManagementSystem.Models.Entities;
using Sprache;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models.Entities
{
    public class EmpHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateEmployed { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateRelieved { get; set; }

        [Required]
        [StringLength(500)] 
        public required string Duration { get; set; }

        [Required]
        public bool CurrentlyEmployed { get; set; }

        [Required]
        [StringLength(1000)] 
        public required string Description { get; set; }

        //public int? DepartmentId { get; set; }
        //[ForeignKey("DepartmentId")]
        //public Department? Department { get; set; }

        //public int? PositionId { get; set; }
        //[ForeignKey("PositionId")]
        //public EmpPositions? Position { get; set; }
        public List<EmpPositions> PositionsHeld { get; set; } = new List<EmpPositions>();
    }
}


