using Microsoft.EntityFrameworkCore.Migrations;

namespace Quiz.Data.Migrations
{
    public partial class UserToQuestionsAndAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Questions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Answers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_IdentityUserId",
                table: "Questions",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_IdentityUserId",
                table: "Answers",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_AspNetUsers_IdentityUserId",
                table: "Answers",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_IdentityUserId",
                table: "Questions",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_AspNetUsers_IdentityUserId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_IdentityUserId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_IdentityUserId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Answers_IdentityUserId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Answers");
        }
    }
}
