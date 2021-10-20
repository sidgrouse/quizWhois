using Microsoft.EntityFrameworkCore.Migrations;

namespace QuizWhois.Domain.Migrations
{
    public partial class Hint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RatingNumber",
                table: "QuestionRating",
                newName: "Value");

            migrationBuilder.CreateTable(
                name: "Hint",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hint_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Login" },
                values: new object[] { 1L, "Qwerty" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Login" },
                values: new object[] { 2L, "Asdfg" });

            migrationBuilder.CreateIndex(
                name: "IX_Hint_QuestionId",
                table: "Hint",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hint");

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "QuestionRating",
                newName: "RatingNumber");
        }
    }
}
