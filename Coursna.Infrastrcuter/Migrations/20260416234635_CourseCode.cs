using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursna.Infrastrcuter.Migrations
{
    /// <inheritdoc />
    public partial class CourseCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsedByStudentId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseCode_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseCode_Code",
                table: "CourseCode",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseCode_CourseId",
                table: "CourseCode",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseCode");
        }
    }
}
