using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedDeptNameToPosition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emp_Positions_Departments_DepartmentId",
                table: "Emp_Positions");

            migrationBuilder.DropIndex(
                name: "IX_Emp_Positions_DepartmentId",
                table: "Emp_Positions");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "Emp_Positions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "Emp_Positions");

            migrationBuilder.CreateIndex(
                name: "IX_Emp_Positions_DepartmentId",
                table: "Emp_Positions",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emp_Positions_Departments_DepartmentId",
                table: "Emp_Positions",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
