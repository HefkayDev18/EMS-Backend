using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedPositionsList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emp_History_Departments_DepartmentId",
                table: "Emp_History");

            migrationBuilder.DropForeignKey(
                name: "FK_Emp_History_Emp_Positions_PositionId",
                table: "Emp_History");

            migrationBuilder.DropIndex(
                name: "IX_Emp_History_DepartmentId",
                table: "Emp_History");

            migrationBuilder.DropIndex(
                name: "IX_Emp_History_PositionId",
                table: "Emp_History");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Emp_History");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Emp_History");

            migrationBuilder.AddColumn<int>(
                name: "EmpHistoryId",
                table: "Emp_Positions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Emp_Positions_EmpHistoryId",
                table: "Emp_Positions",
                column: "EmpHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emp_Positions_Emp_History_EmpHistoryId",
                table: "Emp_Positions",
                column: "EmpHistoryId",
                principalTable: "Emp_History",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emp_Positions_Emp_History_EmpHistoryId",
                table: "Emp_Positions");

            migrationBuilder.DropIndex(
                name: "IX_Emp_Positions_EmpHistoryId",
                table: "Emp_Positions");

            migrationBuilder.DropColumn(
                name: "EmpHistoryId",
                table: "Emp_Positions");

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Emp_History",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "Emp_History",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Emp_History_DepartmentId",
                table: "Emp_History",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Emp_History_PositionId",
                table: "Emp_History",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emp_History_Departments_DepartmentId",
                table: "Emp_History",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emp_History_Emp_Positions_PositionId",
                table: "Emp_History",
                column: "PositionId",
                principalTable: "Emp_Positions",
                principalColumn: "PositionId");
        }
    }
}
