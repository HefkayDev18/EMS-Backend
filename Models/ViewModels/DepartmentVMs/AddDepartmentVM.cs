namespace EmployeeManagementSystem.Models.ViewModels.DepartmentVMs
{
    public class AddDepartmentVM
    {
        public required string DepartmentName { get; set; }
        public required string Description { get; set; }
        public required string HeadOfDepartment { get; set; }
        public required string FacultyName { get; set; }
    }

    public class AddFacultyVM
    {
        public required string FacultyName { get; set; }
    }
}
