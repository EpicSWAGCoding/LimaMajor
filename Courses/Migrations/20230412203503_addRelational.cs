using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Courses.Migrations
{
    public partial class addRelational : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "wallets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "lessons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Cost",
                table: "courses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                table: "courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_wallets_UserId",
                table: "wallets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_lessons_CourseId",
                table: "lessons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_courses_ManagerId",
                table: "courses",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_courses_managers_ManagerId",
                table: "courses",
                column: "ManagerId",
                principalTable: "managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lessons_courses_CourseId",
                table: "lessons",
                column: "CourseId",
                principalTable: "courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_wallets_users_UserId",
                table: "wallets",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_managers_ManagerId",
                table: "courses");

            migrationBuilder.DropForeignKey(
                name: "FK_lessons_courses_CourseId",
                table: "lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_wallets_users_UserId",
                table: "wallets");

            migrationBuilder.DropIndex(
                name: "IX_wallets_UserId",
                table: "wallets");

            migrationBuilder.DropIndex(
                name: "IX_lessons_CourseId",
                table: "lessons");

            migrationBuilder.DropIndex(
                name: "IX_courses_ManagerId",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "wallets");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "lessons");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "courses");

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
