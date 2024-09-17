using EmployeeManagementSystem.Models.Entities;
using EmployeeManagementSystem.Models.ViewModels.DepartmentVMs;
using EmployeeManagementSystem.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FacultiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FacultiesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetFaculties")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Faculty>>> GetFaculties()
        {
            var faculties = await _unitOfWork.Faculties.GetAllFacultiesAsync();
            return Ok(faculties);
        }

        [HttpGet("GetFacultyById/{id}")]
        [Authorize(Roles = "Faculty_Officer")]
        public async Task<ActionResult<Faculty>> GetFacultiesById(int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var faculties = await _unitOfWork.Faculties.GetFacultyByIdAsync(id);

            if (faculties == null)
                return NotFound();

            return Ok(faculties);
        }

        [HttpPost("AddFaculty")]
        [Authorize(Roles = "Faculty_Officer")]
        public async Task<ActionResult<Department>> AddFaculty(AddFacultyVM addFacultyVM)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingFaculty = await _unitOfWork.Faculties.GetFacultyByNameAsync(addFacultyVM.FacultyName);

            if (existingFaculty != null)
            {
                ModelState.AddModelError("FacultyName", "Faculty name already exists");
                return BadRequest(ModelState);
            }

            var newFaculty = new Faculty()
            {
                FacultyName = addFacultyVM.FacultyName
            };


            await _unitOfWork.Faculties.AddFacultyAsync(newFaculty);
            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetFacultiesById), new { id = newFaculty.FacultyId }, newFaculty);

        }


        [HttpDelete("DeleteFaculty/{id}")]
        [Authorize(Roles = "Faculty_Officer")]
        public async Task<IActionResult> DeleteFaculty(int id)
        {

            var faculty = await _unitOfWork.Faculties.GetFacultyByIdAsync(id);

            if (faculty == null)
                return NotFound();

            await _unitOfWork.Faculties.DeleteFacultyAsync(faculty.FacultyId);
            await _unitOfWork.CompleteAsync();

            return Ok("Faculty deleted successfully");
        }
    }
}
