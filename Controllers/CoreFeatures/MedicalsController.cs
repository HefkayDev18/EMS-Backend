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
    public class MedicalsController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        [HttpGet("GetMedicalRecords")]
        [Authorize(Roles = "MED_Admin")]
        public async Task<IActionResult> GetMedicalRecords()
        {
            var records = await _unitOfWork.Emp_MedRecords.GetMedRecordsAsync();
            return Ok(records);
        }

        [HttpGet("GetMedicalRecordsByEmployee/{employeeId}")]
        [Authorize]
        public async Task<IActionResult> GetMedicalRecordsByEmployee(int employeeId)
        {
            var records = await _unitOfWork.Emp_MedRecords.GetMedRecordsByEmployeeIdAsync(employeeId);
            if (records == null)
            {
                return NotFound("No medical records found for this employee.");
            }
            return Ok(records);
        }

        //[HttpPost("LodgeComplaint")]
        //[Authorize(Roles = "Admin, User")]
        //public async Task<IActionResult> LodgeComplaint([FromBody] LodgeMedRecordVM request)
        //{
        //    var record = new EmpMedRecords
        //    {
        //        EmployeeId = request.EmployeeId,
        //        Diagnosis = request.Diagnosis,
        //        Prescription = request.Prescription,
        //        DateOfRecord = DateTime.Now,
        //        AppointmentDate = request.AppointmentDate,
        //        DoctorName = request.DoctorName,
        //        Comments = request.Comments
        //    };

        //    var addedRecord = await _unitOfWork.Emp_MedRecords.AddMedRecordAsync(record);
        //    return CreatedAtAction(nameof(GetMedicalRecordsByEmployee), new { employeeId = request.EmployeeId }, addedRecord);
        //}


        [HttpPost("AddMedicalRecord/{employeeId}")]
        [Authorize(Roles = "MED_Admin")]
        public async Task<IActionResult> AddMedicalRecord(int employeeId, [FromBody] AddMedRecordVM request)
        {
            var employee = _unitOfWork.Employees.GetEmployeeByIdAsync(employeeId);

            if (employee != null)
            {
                var record = new EmpMedRecords
                {
                    EmployeeId = employeeId,
                    Diagnosis = request.Diagnosis,
                    Prescription = request.Prescription,
                    DateOfRecord = DateTime.Now,
                    AppointmentDate = request.AppointmentDate,
                    DoctorName = request.DoctorName,
                    Comments = request.Comments
                };

                var addedRecord = await _unitOfWork.Emp_MedRecords.AddMedRecordAsync(record);
                return CreatedAtAction(nameof(GetMedicalRecordsByEmployee), new { employeeId }, addedRecord);
            }

            return NotFound();
        }

        [HttpGet("GetPaginatedMedicalRecords")]
        [Authorize(Roles = "MED_Admin")]
        public async Task<IActionResult> GetPaginatedMedicalRecords([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var records = await _unitOfWork.Emp_MedRecords.GetPaginatedMedRecordsAsync(page, pageSize);
            return Ok(records);
        }


    }
}
