using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Models.ViewModels.DepartmentVMs;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetDepartments")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            var departments = await _unitOfWork.Departments.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        [HttpGet("GetDepartmentById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Department>> GetDepartmentById(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var department = await _unitOfWork.Departments.GetDepartmentByIdAsync(id);

            if (department == null)
                return NotFound();

            return Ok(department);
        }

        [HttpPost("AddDepartment")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Department>> AddDepartment(AddDepartmentVM addDepartmentVM)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDepartment = await _unitOfWork.Departments.GetDepartmentByNameAsync(addDepartmentVM.DepartmentName);

            if (existingDepartment != null)
            {
                ModelState.AddModelError("DepartmentName", "Department name already exists");
                return BadRequest(ModelState);
            }

            var existingFaculty = await _unitOfWork.Faculties.GetFacultyByNameAsync(addDepartmentVM.FacultyName);

            if (existingFaculty != null)
            {
                var newDepartment = new Department()
                {
                    DepartmentName = addDepartmentVM.DepartmentName,
                    Description = addDepartmentVM.Description,
                    HeadOfDepartment = addDepartmentVM.HeadOfDepartment,
                    FacultyName = addDepartmentVM.FacultyName
                };


                await _unitOfWork.Departments.AddDepartmentAsync(newDepartment);
                await _unitOfWork.CompleteAsync();

                return CreatedAtAction(nameof(GetDepartmentById), new { id = newDepartment.DepartmentId }, newDepartment);
            }

            return NotFound("Faculty Entered does not exist");
        }


        [HttpPut("UpdateDepartment/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Department>> UpdateDepartment(int id, UpdateDepartmentVM updateDepartmentVM)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var departmentWithId = await _unitOfWork.Departments.GetDepartmentByIdAsync(id);

            if (departmentWithId == null)
            {
                return NotFound();
            }

            departmentWithId.DepartmentName = updateDepartmentVM.DepartmentName;
            departmentWithId.Description = updateDepartmentVM.Description;
            departmentWithId.HeadOfDepartment = updateDepartmentVM.HeadOfDepartment;

            await _unitOfWork.Departments.UpdateDepartmentAsync(departmentWithId);
            await _unitOfWork.CompleteAsync();

            return Ok("Department name updated successfully");
        }

        [HttpDelete("DeleteDepartment/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {

            var department = await _unitOfWork.Departments.GetDepartmentByIdAsync(id);

            if (department == null)
                return NotFound();

            await _unitOfWork.Departments.DeleteDepartmentAsync(department.DepartmentId);
            await _unitOfWork.CompleteAsync();

            return Ok("Department deleted successfully");
        }


    }
}
