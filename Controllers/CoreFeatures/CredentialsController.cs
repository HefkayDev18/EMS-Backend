using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Models.ViewModels.CoreFeaturesVMs;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace EmployeeManagementSystem.Controllers.CoreFeatures
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CredentialsController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        [HttpGet("GetEmpUploads/{empId}")]
        [Authorize]
        public async Task<IActionResult> GetEmpUploads(int empId)
        {
            var uploads = await _unitOfWork.Emp_Credentials.GetUploadsByEmployeeIdAsync(empId);
            return Ok(uploads);
        }

        [HttpGet("GetAllUploads")]
        [Authorize(Roles = "HR_Admin")]
        public async Task<IActionResult> GetAllUploads()
        {
            var uploads = await _unitOfWork.Emp_Credentials.GetUploadsAsync();
            return Ok(uploads);
        }


        [HttpPost("Uploads/{empId}")]
        [Authorize]
        public async Task<IActionResult> Uploads(int empId, [FromForm] CredentialsVM model)
        {
            if (model.Files == null || model.Files.Count == 0)
            {
                return BadRequest("No files uploaded.");
            }

            var employee = await _unitOfWork.Employees.GetEmployeeByIdAsync(empId);

            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            var uploadedFilePaths = new List<string>();

            foreach (var file in model.Files)
            {
                if (file.Length == 0)
                {
                    return BadRequest("One or more files are empty.");
                }

                var filePath = Path.Combine(uploadsDirectory, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var credential = new Credentials
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeCred = employee,
                    DocumentType = model.DocumentType,
                    Description = model.Description,
                    FilePath = filePath,
                    UploadedDate = DateTime.UtcNow
                };

                await _unitOfWork.Emp_Credentials.AddFileAsync(credential);
                uploadedFilePaths.Add(filePath);
            }

            await _unitOfWork.CompleteAsync();

            return Ok(new { Message = "Files uploaded successfully.", Files = uploadedFilePaths });
        }

        //public async Task<IActionResult> Uploads(int empId, [FromForm] CredentialsVM model)
        //{
        //    if (model.File == null || model.File.Length == 0)
        //    {
        //        return BadRequest("No file uploaded.");
        //    }

        //    var employee = await _unitOfWork.Employees.GetEmployeeByIdAsync(empId);

        //    var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        //    if (!Directory.Exists(uploadsDirectory))
        //    {
        //        Directory.CreateDirectory(uploadsDirectory);
        //    }

        //    var filePath = Path.Combine(uploadsDirectory, model.File.FileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await model.File.CopyToAsync(stream);
        //    }

        //    var credential = new Credentials
        //    {
        //        EmployeeId = employee.EmployeeId,
        //        EmployeeCred = employee,
        //        DocumentType = model.DocumentType,
        //        Description = model.Description,
        //        FilePath = filePath,
        //        UploadedDate = DateTime.UtcNow
        //    };

        //    await _unitOfWork.Emp_Credentials.AddFileAsync(credential);
        //    await _unitOfWork.CompleteAsync();

        //    return Ok("File uploaded successfully.");
        //}
    }
}
