using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedPositionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Emp_Positions",
                columns: table => new
                {
                    PositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emp_Positions", x => x.PositionId);
                    table.ForeignKey(
                        name: "FK_Emp_Positions_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emp_History_DepartmentId",
                table: "Emp_History",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Emp_History_PositionId",
                table: "Emp_History",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Emp_Positions_DepartmentId",
                table: "Emp_Positions",
                column: "DepartmentId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emp_History_Departments_DepartmentId",
                table: "Emp_History");

            migrationBuilder.DropForeignKey(
                name: "FK_Emp_History_Emp_Positions_PositionId",
                table: "Emp_History");

            migrationBuilder.DropTable(
                name: "Emp_Positions");

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
        }
    }
}
