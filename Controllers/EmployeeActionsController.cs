using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Models.ViewModels.DepartmentVMs;
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
    public class EmployeeActionsController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [HttpGet("GetAllEmployees")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _unitOfWork.Employees.GetAllEmployeesAsync();

            return Ok(employees);
        }


        [HttpGet("GetEmployeeById/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _unitOfWork.Employees.GetEmployeeByIdAsync(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }


        [HttpPost("AddEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee(AddEmployeeVM employeeVM)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingEmployee = await _unitOfWork.Employees.GetEmployeeByEmailAsync(employeeVM.Email);

            if (existingEmployee != null)
            {
                ModelState.AddModelError("Email", "An employee with this email already exists.");
                return BadRequest(ModelState);
            }

            var existingDepartment = await _unitOfWork.Departments.GetDepartmentByNameAsync(employeeVM.Department);

           if(existingDepartment != null)
            {
                var newEmployee = new Employee()
                {
                    FullName = employeeVM.FullName,
                    Email = employeeVM.Email,
                    Department = employeeVM.Department,
                    PhoneNumber = employeeVM.PhoneNumber,
                    Gender = employeeVM.Gender,
                    Position = employeeVM.Position,
                    Salary = employeeVM.Salary,
                    DateCreated = DateTime.Now,
                    DateOfBirth = employeeVM.DateOfBirth,
                    IsActive = true
                };

                await _unitOfWork.Employees.AddEmployeeAsync(newEmployee);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.EmployeeId }, newEmployee);
            }

            return NotFound("Department does not exist");
        }


        [HttpPut("UpdateEmployeeRole/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployeeRole(int id, UpdateEmployeeRoleVM updateEmployeeRoleVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeWithId = await _unitOfWork.Employees.GetEmployeeByIdAsync(id);
            var userWithId = await _unitOfWork.Sys_Users.GetUserByEmployeeIdAsync(id);

            if (employeeWithId == null || userWithId == null)
            {
                return NotFound();
            }

            employeeWithId.RoleId = updateEmployeeRoleVM.RoleId;
            userWithId.RoleId = updateEmployeeRoleVM.RoleId;

            await _unitOfWork.Employees.UpdateEmployeeAsync(employeeWithId);
            _unitOfWork.Sys_Users.Update(userWithId);

            await _unitOfWork.CompleteAsync();

            var appUser = await _userManager.FindByIdAsync(userWithId.IdentityId.ToString());

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(appUser);
            var removeRoleResult = await _userManager.RemoveFromRolesAsync(appUser, currentRoles);

            if (!removeRoleResult.Succeeded)
            {
                return StatusCode(500, "Error removing user roles.");
            }

            var addRoleResult = await _userManager.AddToRoleAsync(appUser, updateEmployeeRoleVM.RoleName);

            if (!addRoleResult.Succeeded)
            {
                return StatusCode(500, "Error adding user role.");
            }

            return Ok("Role updated successfully");
        }


        [HttpPut("UpdateEmployee/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateEmployee(int id, UpdateEmployeeVM UpdateEmployee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeWithId = await _unitOfWork.Employees.GetEmployeeByIdAsync(id);

            if(employeeWithId is null)
            {
                return NotFound();
            }

            var existingEmployeeWithEmail = await _unitOfWork.Employees.GetEmployeeByEmailAsync(UpdateEmployee.Email);

            if (existingEmployeeWithEmail != null && existingEmployeeWithEmail.EmployeeId != id)
            {
                ModelState.AddModelError("Email", "Another employee with this email already exists.");
                return BadRequest(ModelState);
            }
            //if (id != employee.EmployeeId)
            //    return BadRequest();

            employeeWithId.FullName = UpdateEmployee.FullName;
            employeeWithId.Email = UpdateEmployee.Email;
            employeeWithId.Department = UpdateEmployee.Department;
            employeeWithId.PhoneNumber = UpdateEmployee.PhoneNumber;
            employeeWithId.Position = UpdateEmployee.Position;
            employeeWithId.Salary = UpdateEmployee.Salary;
            employeeWithId.IsActive = UpdateEmployee.IsActive;


            await _unitOfWork.Employees.UpdateEmployeeAsync(employeeWithId);
            await _unitOfWork.CompleteAsync();

            //return NoContent();
            return Ok(employeeWithId);
        }


        [HttpDelete("DeleteEmployee/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _unitOfWork.Employees.GetEmployeeByIdAsync(id);

            if (employee == null)
                return NotFound();

            await _unitOfWork.Employees.DeleteEmployeeAsync(employee.EmployeeId);
            await _unitOfWork.CompleteAsync();

            return Ok("Employee deleted successfully");
        }
    }
}

