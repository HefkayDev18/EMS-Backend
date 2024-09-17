using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Models.ViewModels.CoreFeaturesVMs;
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
    [Authorize]
    public class EmployeeActionsController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;


        #region User
        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "Admin, HR_Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.Sys_Users.GetAllUsersAsync();
            return Ok(users);
        }
        #endregion

        #region Employee

        [HttpGet("GetAllEmployees")]
        [Authorize(Roles = "Admin, HR_Admin, MED_Admin")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _unitOfWork.Employees.GetAllEmployeesAsync();

            if (employees == null)
            {
                return NotFound();
            }
            return Ok(employees);
        }

        [HttpGet("GetEmployeeById/{id}")]
        [Authorize]
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
        [Authorize(Roles = "HR_Admin")]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeVM employeeVM)
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

            if (existingDepartment != null)
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
                    IsActive = false
                };

                await _unitOfWork.Employees.AddEmployeeAsync(newEmployee);

                var newPosition = new EmpPositions()
                {
                    EmployeeId = newEmployee.EmployeeId,
                    Position = newEmployee.Position,
                    DateStarted = DateTime.Now,
                    DepartmentId = existingDepartment.DepartmentId,
                    DepartmentName = existingDepartment.DepartmentName
                };
                await _unitOfWork.Emp_Positions.AddPositionAsync(newPosition);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.EmployeeId }, newEmployee);
            }

            return NotFound("Department does not exist");
        }


        [HttpPut("UpdateEmployee/{id}")]
        [Authorize(Roles = "HR_Admin")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeVM UpdateEmployee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeWithId = await _unitOfWork.Employees.GetEmployeeByIdAsync(id);

            if (employeeWithId is null)
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

            var existingDepartment = await _unitOfWork.Departments.GetDepartmentByNameAsync(UpdateEmployee.Department);

            if (existingDepartment != null)
            {
                if(employeeWithId.Position != UpdateEmployee.Position)
                {
                    var newPosition = new EmpPositions()
                    {
                        EmployeeId = employeeWithId.EmployeeId,
                        Position = UpdateEmployee.Position,
                        DateStarted = DateTime.Now,
                        DepartmentId = existingDepartment.DepartmentId,
                        DepartmentName = existingDepartment.DepartmentName
                    };

                    await _unitOfWork.Emp_Positions.AddPositionAsync(newPosition);
                }

                employeeWithId.FullName = UpdateEmployee.FullName;
                employeeWithId.Email = UpdateEmployee.Email;
                employeeWithId.Department = UpdateEmployee.Department;
                employeeWithId.PhoneNumber = UpdateEmployee.PhoneNumber;
                employeeWithId.Position = UpdateEmployee.Position;
                employeeWithId.Salary = UpdateEmployee.Salary;
                employeeWithId.IsActive = UpdateEmployee.IsActive;

                await _unitOfWork.Employees.UpdateEmployeeAsync(employeeWithId);             

                await _unitOfWork.CompleteAsync();

                return Ok(employeeWithId);
            }

            //return NoContent();
            return NotFound();
        }

        [HttpDelete("DeleteEmployee/{id}")]
        [Authorize(Roles = "HR_Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _unitOfWork.Employees.GetEmployeeByIdAsync(id);

            if (employee == null)
                return NotFound();

            await _unitOfWork.Employees.DeleteEmployeeAsync(employee.EmployeeId);
            await _unitOfWork.CompleteAsync();

            return Ok("Employee deleted successfully");
        }

        #endregion

        #region Roles
        [HttpGet("GetAllRoles")]
        [Authorize(Roles = "Admin, HR_Admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _unitOfWork.Sys_Roles.GetAllRolesAsync();

            return Ok(roles);
        }

        [HttpGet("GetUserRole/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserRole(int id)
        {
            var user = await _unitOfWork.Sys_Users.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roles = await _unitOfWork.Sys_Roles.GetRoleByIdAsync(user.RoleId);
            var role = roles.RoleName;

            return Ok(new { role });
        }


        [HttpPut("UpdateEmployeeRole/{id}")]
        [Authorize(Roles = "HR_Admin")]
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
            if (currentRoles.Any())
            {
                var removeRoleResult = await _userManager.RemoveFromRolesAsync(appUser, currentRoles);
                if (!removeRoleResult.Succeeded)
                {
                    return StatusCode(500, "Error removing user roles.");
                }
            }

            var addRoleResult = await _userManager.AddToRoleAsync(appUser, updateEmployeeRoleVM.RoleName);

            if (!addRoleResult.Succeeded)
            {
                return StatusCode(500, "Error adding user role.");
            }

            return Ok("Role updated successfully");
        }



        #endregion


    }
}

