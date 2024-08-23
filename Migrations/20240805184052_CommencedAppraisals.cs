using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class CommencedAppraisals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Emp_Appraisals",
                columns: table => new
                {
                    AppraisalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    AppDateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppDateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicationProgress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Teaching = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatentConferencing = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommunityService = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdministrationExperience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Communication = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Teamwork = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Leadership = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProblemSolving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Punctuality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adaptability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverallSatisfaction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emp_Appraisals", x => x.AppraisalId);
                    table.ForeignKey(
                        name: "FK_Emp_Appraisals_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Emp_AppraisalsComment",
                columns: table => new
                {
                    AppraisalCommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppraisalId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedByEmployeeEmployeeId = table.Column<int>(type: "int", nullable: true),
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
                name: "IX_Emp_Appraisals_EmployeeId",
                table: "Emp_Appraisals",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Emp_AppraisalsComment_AppraisalId",
                table: "Emp_AppraisalsComment",
                column: "AppraisalId");

            migrationBuilder.CreateIndex(
                name: "IX_Emp_AppraisalsComment_CreatedByEmployeeEmployeeId",
                table: "Emp_AppraisalsComment",
                column: "CreatedByEmployeeEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emp_AppraisalsComment");

            migrationBuilder.DropTable(
                name: "Emp_Appraisals");
        }
    }
}
