using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursna.Infrastrcuter.Migrations
{
    /// <inheritdoc />
    public partial class GradeChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "gradeId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_gradeId",
                table: "AspNetUsers",
                column: "gradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Grades_gradeId",
                table: "AspNetUsers",
                column: "gradeId",
                principalTable: "Grades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Grades_gradeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_gradeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "gradeId",
                table: "AspNetUsers");
        }
    }
}
