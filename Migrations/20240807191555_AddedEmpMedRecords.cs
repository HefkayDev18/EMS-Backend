using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmpMedRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emp_AppraisalsComment");

            migrationBuilder.CreateTable(
                name: "Emp_MedRecords",
                columns: table => new
                {
                    MedRecordsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfRecord = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emp_MedRecords", x => x.MedRecordsId);
                    table.ForeignKey(
                        name: "FK_Emp_MedRecords_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emp_MedRecords_EmployeeId",
                table: "Emp_MedRecords",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emp_MedRecords");

            migrationBuilder.CreateTable(
                name: "Emp_AppraisalsComment",
                columns: table => new
                {
                    AppraisalCommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppraisalId = table.Column<int>(type: "int", nullable: false),
                    CreatedByEmployeeEmployeeId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emp_AppraisalsComment", x => x.AppraisalCommentId);
                    table.ForeignKey(
                        name: "FK_Emp_AppraisalsComment_Emp_Appraisals_AppraisalId",
                        column: x => x.AppraisalId,
                        principalTable: "Emp_Appraisals",
                        principalColumn: "AppraisalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Emp_AppraisalsComment_Employees_CreatedByEmployeeEmployeeId",
                        column: x => x.CreatedByEmployeeEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emp_AppraisalsComment_AppraisalId",
                table: "Emp_AppraisalsComment",
                column: "AppraisalId");

            migrationBuilder.CreateIndex(
                name: "IX_Emp_AppraisalsComment_CreatedByEmployeeEmployeeId",
                table: "Emp_AppraisalsComment",
                column: "CreatedByEmployeeEmployeeId");
        }
    }
}
