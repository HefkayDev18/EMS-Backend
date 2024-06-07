using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Sys_Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Sys_Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sys_Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Users_EmployeeId",
                table: "Sys_Users",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sys_Users_RoleId",
                table: "Sys_Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Sys_Roles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "Sys_Roles",
                principalColumn: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Sys_Users_UserId",
                table: "Employees",
                column: "UserId",
                principalTable: "Sys_Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sys_Users_Employees_EmployeeId",
                table: "Sys_Users",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sys_Users_Sys_Roles_RoleId",
                table: "Sys_Users",
                column: "RoleId",
                principalTable: "Sys_Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Sys_Roles_RoleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Sys_Users_UserId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Sys_Users_Employees_EmployeeId",
                table: "Sys_Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Sys_Users_Sys_Roles_RoleId",
                table: "Sys_Users");

            migrationBuilder.DropTable(
                name: "Sys_Roles");

            migrationBuilder.DropIndex(
                name: "IX_Sys_Users_EmployeeId",
                table: "Sys_Users");

            migrationBuilder.DropIndex(
                name: "IX_Sys_Users_RoleId",
                table: "Sys_Users");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RoleId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Sys_Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Sys_Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Employees");
        }
    }
}
