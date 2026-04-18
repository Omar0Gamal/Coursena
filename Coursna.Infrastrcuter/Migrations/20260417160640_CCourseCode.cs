using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursna.Infrastrcuter.Migrations
{
    /// <inheritdoc />
    public partial class CCourseCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseCode_Courses_CourseId",
                table: "CourseCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseCode",
                table: "CourseCode");

            migrationBuilder.RenameTable(
                name: "CourseCode",
                newName: "courseCodes");

            migrationBuilder.RenameIndex(
                name: "IX_CourseCode_CourseId",
                table: "courseCodes",
                newName: "IX_courseCodes_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseCode_Code",
                table: "courseCodes",
                newName: "IX_courseCodes_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_courseCodes",
                table: "courseCodes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_courseCodes_Courses_CourseId",
                table: "courseCodes",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courseCodes_Courses_CourseId",
                table: "courseCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_courseCodes",
                table: "courseCodes");

            migrationBuilder.RenameTable(
                name: "courseCodes",
                newName: "CourseCode");

            migrationBuilder.RenameIndex(
                name: "IX_courseCodes_CourseId",
                table: "CourseCode",
                newName: "IX_CourseCode_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_courseCodes_Code",
                table: "CourseCode",
                newName: "IX_CourseCode_Code");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseCode",
                table: "CourseCode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseCode_Courses_CourseId",
                table: "CourseCode",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
