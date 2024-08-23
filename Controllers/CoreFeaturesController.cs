using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Models.ViewModels.CoreFeaturesVMs;
using EmployeeManagementSystem.Models.ViewModels.EmployeeVMs;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoreFeaturesController (UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;


        [HttpGet("GetEmployeeHistory/{employeeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetEmployeeHistory(int employeeId)
        {
            var empHistories = await _unitOfWork.Emp_History.GetEmploymentHistoriesByEmployeeIdAsync(employeeId);
            var positionsHeld = await _unitOfWork.Emp_Positions.GetPositionsByEmployeeIdAsync(employeeId);


            if (!empHistories.Any() && !positionsHeld.Any())
            {
                return NotFound("No details found for this employee");
            }

            var employeeDetails = new
            {
                EmploymentHistories = empHistories.Select(eh => new ViewEmpHistoryVM
                {
                    Id = eh.Id,
                    DateEmployed = eh.DateEmployed,
                    DateRelieved = eh.DateRelieved,
                    Duration = eh.Duration,
                    CurrentlyEmployed = eh.CurrentlyEmployed,
                    Description = eh.Description,
                    PositionsHeld = positionsHeld
                })
            };

            return Ok(employeeDetails);
        }


        [HttpPost("CreateEmploymentHistory/{employeeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEmploymentHistory(int employeeId,[FromBody] AddEmpHistoryVM empHistoryVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingEmployee = await _unitOfWork.Employees.GetEmployeeByIdAsync(employeeId);
            var existingHistory = await _unitOfWork.Emp_History.GetEmpHistoryByIdAsync(employeeId);
            var positionsHeld = await _unitOfWork.Emp_Positions.GetPositionsByEmployeeIdAsync(employeeId);
            

            if (existingEmployee == null || existingHistory != null)
            {
                return NotFound("Employee does not exist or already has an employment history");
            }

            var duration = CalculateDuration(empHistoryVM.DateEmployed, DateTime.Now);

            var newHistory = new EmpHistory()
            { 
                EmployeeId = employeeId,
                DateEmployed = empHistoryVM.DateEmployed,
                CurrentlyEmployed = empHistoryVM.CurrentlyEmployed,
                Duration = duration,
                Description = empHistoryVM.Description,
                PositionsHeld = positionsHeld
                //PositionId = empHistoryVM.PositionId,
                //DepartmentId = empHistoryVM.DepartmentId 
            };

            await _unitOfWork.Emp_History.AddEmployeeHistoryAsync(newHistory);

            existingEmployee.HasEmploymentHistory = true;
            await _unitOfWork.Employees.UpdateEmployeeAsync(existingEmployee);

            await _unitOfWork.CompleteAsync();

            return Ok(newHistory);
        }

        [HttpPut("UpdateEmploymentHistory/{employeeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmploymentHistory(int employeeId,[FromBody] UpdateEmpHistoryVM empHistoryVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (empHistoryVM.CurrentlyEmployed && empHistoryVM.DateRelieved.HasValue)
            {
                ModelState.AddModelError(nameof(empHistoryVM.DateRelieved), "DateRelieved should be null if CurrentlyEmployed is true.");
            }

            if (!empHistoryVM.CurrentlyEmployed && !empHistoryVM.DateRelieved.HasValue)
            {
                ModelState.AddModelError(nameof(empHistoryVM.DateRelieved), "DateRelieved must be provided if CurrentlyEmployed is false.");
            }

            var existingHistory = await _unitOfWork.Emp_History.GetEmpHistoryByIdAsync(employeeId);

            if (existingHistory == null)
            {
                return NotFound("Employment history does not exist");
            }

            var existingEmployee = await _unitOfWork.Employees.GetEmployeeByIdAsync(employeeId);

            if (existingEmployee == null)
            {
                return NotFound("Employee does not exist");
            }

            
            existingHistory.DateEmployed = empHistoryVM.DateEmployed;
            existingHistory.DateRelieved = empHistoryVM.DateRelieved;
            existingHistory.CurrentlyEmployed = empHistoryVM.CurrentlyEmployed;
            existingHistory.Description = empHistoryVM.Description;


            await _unitOfWork.Emp_History.UpdateEmpHistoryAsync(existingHistory);
            await _unitOfWork.CompleteAsync();

            return Ok(existingHistory);
        }


        #region Private functions
        //private string CalculateDuration(DateTime startDate, DateTime endDate)
        //{
        //    var years = endDate.Year - startDate.Year;
        //    var months = endDate.Month - startDate.Month;
        //    var days = endDate.Day - startDate.Day;

        //    if (days < 0)
        //    {
        //        months--;
        //        days += DateTime.DaysInMonth(endDate.Year, endDate.Month);
        //    }

        //    if (months < 0)
        //    {
        //        years--;
        //        months += 12;
        //    }

        //    return $"{years} years {months} months {days} days";
        //}

        private string CalculateDuration(DateTime startDate, DateTime endDate)
        {
            var years = endDate.Year - startDate.Year;
            var months = endDate.Month - startDate.Month;
            var days = endDate.Day - startDate.Day;

            if (days < 0)
            {
                months--;
                days += DateTime.DaysInMonth(endDate.Year, endDate.Month);
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            var durationParts = new List<string>();

            if (years > 0)
            {
                durationParts.Add($"{years} years");
            }

            if (months > 0)
            {
                durationParts.Add($"{months} months");
            }

            if (days > 0)
            {
                durationParts.Add($"{days} days");
            }

            return durationParts.Any() ? string.Join(" ", durationParts) : "0 days";
        }

        #endregion
    }
}
