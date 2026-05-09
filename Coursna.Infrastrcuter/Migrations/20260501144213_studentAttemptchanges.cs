using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coursna.Infrastrcuter.Migrations
{
    /// <inheritdoc />
    public partial class studentAttemptchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_QuizAttempts_quizAttemptId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_quizAttemptId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "quizAttemptId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "QuizAttempts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_StudentId",
                table: "QuizAttempts",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_StudentId",
                table: "QuizAttempts",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_StudentId",
                table: "QuizAttempts");

            migrationBuilder.DropIndex(
                name: "IX_QuizAttempts_StudentId",
                table: "QuizAttempts");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "QuizAttempts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "quizAttemptId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_quizAttemptId",
                table: "AspNetUsers",
                column: "quizAttemptId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_QuizAttempts_quizAttemptId",
                table: "AspNetUsers",
                column: "quizAttemptId",
                principalTable: "QuizAttempts",
                principalColumn: "Id");
        }
    }
}
