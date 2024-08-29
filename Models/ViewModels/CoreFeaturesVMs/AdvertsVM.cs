using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models.ViewModels.CoreFeaturesVMs
{
    public class AddAdvertsVM
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Link { get; set; }
        public bool IsAdActive { get; set; }

        [ValidateLinkOrImage]
        public IFormFile? ImageFile { get; set; }
    }

    public class UpdateAdvertsVM
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Link { get; set; }
        public bool IsAdActive { get; set; }
        public IFormFile? ImageFile { get; set; }
    }



    public class ValidateLinkOrImage : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (AddAdvertsVM)validationContext.ObjectInstance;

            if (string.IsNullOrEmpty(model.Link) && model.ImageFile == null)
            {
                return new ValidationResult("Either a link or an image file must be provided.");
            }

            return ValidationResult.Success!;
        }
    }
}
