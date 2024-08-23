using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Repositories.Interfaces;
using EmployeeManagementSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.Models.ViewModels.CoreFeaturesVMs;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagementSystem.Controllers.CoreFeatures
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppraisalsController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork /*, INotificationService notification*/) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        //private readonly INotificationService _notification = notification;


        [HttpGet("GetAppraisals")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAppraisals([FromQuery] string status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var pagedResult = await _unitOfWork.Emp_Appraisals.GetAppraisalsByStatusAsync(status, page, pageSize);

            var appraisals = pagedResult.Items;

            foreach (var appraisal in appraisals)
            {
                var employee = await _unitOfWork.Employees.GetEmployeeByIdAsync(appraisal.EmployeeId);
                appraisal.Employee = employee;
            }

            return Ok(new
            {
                Items = appraisals,
                pagedResult.TotalCount,
                pagedResult.PageSize,
                pagedResult.CurrentPage
            });

        }

        [HttpGet("GetAppraisalsByEmployee/{employeeId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAppraisalsByEmployee(int employeeId)
        {
            var appraisals = await _unitOfWork.Emp_Appraisals.GetAppraisalsByEmployeeIdAsync(employeeId);
            if (appraisals == null || !appraisals.Any())
            {
                return NotFound("No appraisals found for this employee.");
            }
            return Ok(appraisals);
        }

        [HttpGet("GetAppraisalById/{appraisalId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAppraisalById(int appraisalId)
        {
            var appraisal = await _unitOfWork.Emp_Appraisals.GetAppraisalByAppIdAsync(appraisalId);
            if (appraisal == null)
            {
                return NotFound("Appraisal not found.");
            }
            return Ok(appraisal);
        }

        [HttpPost("SubmitAppraisal/{employeeId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> SaveOrSubmitAppraisal(int employeeId, [FromBody] AddAppraisalVM appraisalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var prevMonth = DateTime.UtcNow.AddMonths(-6);

            var recentAppraisal = await _unitOfWork.Emp_Appraisals.GetRecentAppraisalAsync(employeeId, prevMonth);

            if (recentAppraisal != null && recentAppraisal.AppDateSubmitted.HasValue)
            {
                return BadRequest("You cannot submit an appraisal within 6 months of your last submission.");
            }

            var existingAppraisal = await _unitOfWork.Emp_Appraisals.GetAppraisalByEmpIdAsync(employeeId);

            EmpAppraisal appraisalToSave;

            if (existingAppraisal == null)
            {
                appraisalToSave = new EmpAppraisal
                {
                    EmployeeId = employeeId,
                    Status = "Pending",
                    PublicationProgress = appraisalDto.PublicationProgress,
                    Teaching = appraisalDto.Teaching,
                    PatentConferencing = appraisalDto.PatentConferencing,
                    CommunityService = appraisalDto.CommunityService,
                    AdministrationExperience = appraisalDto.AdministrationExperience,
                    Communication = appraisalDto.Communication,
                    Teamwork = appraisalDto.Teamwork,
                    Leadership = appraisalDto.Leadership,
                    ProblemSolving = appraisalDto.ProblemSolving,
                    Punctuality = appraisalDto.Punctuality,
                    Adaptability = appraisalDto.Adaptability,
                    OverallSatisfaction = appraisalDto.OverallSatisfaction,
                    Comments = appraisalDto.Comments,
                    AppDateCreated = DateTime.UtcNow
                };

                await _unitOfWork.Emp_Appraisals.AddAppraisalAsync(appraisalToSave);
            }
            else
            {
                existingAppraisal.PublicationProgress = appraisalDto.PublicationProgress;
                existingAppraisal.Teaching = appraisalDto.Teaching;
                existingAppraisal.PatentConferencing = appraisalDto.PatentConferencing;
                existingAppraisal.CommunityService = appraisalDto.CommunityService;
                existingAppraisal.AdministrationExperience = appraisalDto.AdministrationExperience;
                existingAppraisal.Communication = appraisalDto.Communication;
                existingAppraisal.Teamwork = appraisalDto.Teamwork;
                existingAppraisal.Leadership = appraisalDto.Leadership;
                existingAppraisal.ProblemSolving = appraisalDto.ProblemSolving;
                existingAppraisal.Punctuality = appraisalDto.Punctuality;
                existingAppraisal.Adaptability = appraisalDto.Adaptability;
                existingAppraisal.OverallSatisfaction = appraisalDto.OverallSatisfaction;
                existingAppraisal.Comments = appraisalDto.Comments;

                existingAppraisal.Status = "Pending";
                appraisalToSave = existingAppraisal;

                await _unitOfWork.Emp_Appraisals.UpdateAppraisalAsync(existingAppraisal);
            }

            await _unitOfWork.CompleteAsync();

            return Ok(appraisalToSave);
        }


        [HttpPost("ReviewAppraisal/{appraisalId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReviewAppraisal(int appraisalId, [FromBody] ReviewAppraisalVM reviewDto)
        {
            var appraisal = await _unitOfWork.Emp_Appraisals.GetAppraisalByAppIdAsync(appraisalId);

            if (appraisal == null)
            {
                return NotFound("Appraisal not found.");
            }

            if (string.IsNullOrEmpty(reviewDto.Status))
            {
                return BadRequest("Status is required.");
            }

            if (appraisal.Status == "Approved" || appraisal.Status == "Rejected")
            {
                return BadRequest("Cannot modify an already reviewed appraisal.");
            }

            appraisal.Status = reviewDto.Status;
            appraisal.ManagerComment = reviewDto.ManagerComment;
            appraisal.ReviewedAt = DateTime.UtcNow;

            if (reviewDto.Status == "Approved")
            {
                appraisal.AppDateSubmitted = DateTime.UtcNow;
            }
            else if (reviewDto.Status == "Rejected")
            {
                appraisal.AppDateSubmitted = null;
            }

            await _unitOfWork.Emp_Appraisals.UpdateAppraisalAsync(appraisal);
            await _unitOfWork.CompleteAsync();

            return Ok(appraisal);
        }


    }
}
