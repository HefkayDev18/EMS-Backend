using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Models.ViewModels.CoreFeaturesVMs;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers.CoreFeatures
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdvertisementsController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpGet("GetAllAds")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAllAds()
        {
            var ads = await _unitOfWork.Emp_Adverts.GetAdsAsync();
            return Ok(ads);
        }

        [HttpPost("UploadAds/{empId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadAds(int empId, int adsId, [FromForm] AddAdvertsVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _unitOfWork.Employees.GetEmployeeByIdAsync(empId);

            var adExists = _unitOfWork.Emp_Adverts.AdsExists(adsId);

            if (adExists == true)
            {
                return BadRequest("Advert has already been added");
            }

            if (!string.IsNullOrEmpty(model.ImageUrl))
            {
                var newAds = new Advertisements()
                {
                    EmployeeId = employee.EmployeeId,
                    Title = model.Title,
                    Description = model.Description,
                    ImageUrl = model.ImageUrl,
                    Link = model.Link,
                    CreatedAt = DateTime.UtcNow,
                    IsAdActive = true
                };

                await _unitOfWork.Emp_Adverts.AddAdsAsync(newAds);

                await _unitOfWork.CompleteAsync();

                return Ok(newAds);
            }
            else if (model.ImageFile != null)
            {
                string? imageUrl = null;
                if (model.ImageFile != null)
                {
                    var uploadsFolder = Path.Combine("uploads", "adImages");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    imageUrl = $"/images/ads/{uniqueFileName}";
                }

                var newAd = new Advertisements()
                {
                    EmployeeId = employee.EmployeeId,
                    Title = model.Title,
                    Description = model.Description,
                    ImageUrl = imageUrl,
                    Link = model.Link,
                    CreatedAt = DateTime.UtcNow,
                    IsAdActive = true
                };

                await _unitOfWork.Emp_Adverts.AddAdsAsync(newAd);

                await _unitOfWork.CompleteAsync();

                return Ok(newAd);
            }

            return BadRequest();
        }

        [HttpPut("UpdateAds/{empId}/{adsId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAds(int empId, int adsId, [FromForm] UpdateAdvertsVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _unitOfWork.Employees.GetEmployeeByIdAsync(empId);
            var advertisement = await _unitOfWork.Emp_Adverts.GetAdsByAdsIdAsync(adsId);

            advertisement.EmployeeId = employee.EmployeeId;
            advertisement.UpdatedAt = DateTime.UtcNow;
            advertisement.IsAdActive = model.IsAdActive;
            advertisement.Description = model.Description;
            advertisement.Link = model.Link;

            if (!string.IsNullOrEmpty(model.ImageUrl))
            {
                advertisement.ImageUrl = model.ImageUrl;
            }
            else if (model.ImageFile != null)
            {
                string? imageUrl = null;
                if (model.ImageFile != null)
                {
                    var uploadsFolder = Path.Combine("uploads", "adImages");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    imageUrl = $"/images/ads/{uniqueFileName}";

                    advertisement.ImageUrl = imageUrl;
                }
            }

            await _unitOfWork.Emp_Adverts.UpdateAdsAsync(advertisement);

            await _unitOfWork.CompleteAsync();

            return Ok(advertisement);

        }

        //[HttpDelete("DeleteAds/{adsId}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> DeleteAds(int adsId)
        //{
        //    var advertisement = await _unitOfWork.Emp_Adverts.GetAdsByAdsIdAsync(adsId);

        //    if (advertisement == null)
        //    {
        //        return NotFound();
        //    }

        //    await _unitOfWork.Emp_Adverts.DeleteAdsAsync(advertisement.AdvertisementId);
        //    await _unitOfWork.CompleteAsync();

        //    return Ok("Advertisement deleted successfully");
        //}
    }
}
