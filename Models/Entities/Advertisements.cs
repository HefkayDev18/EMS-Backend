using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.Entities
{
    public class Advertisements
    {
        [Key]
        public int AdvertisementId { get; set; }
        public int EmployeeId { get; set; }
        public Employee? EmployeeObj { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Link { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsAdActive { get; set; }
    }
}

