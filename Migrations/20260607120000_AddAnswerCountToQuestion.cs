using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace quiz_app_api.Migrations
{
    /// <inheritdoc />
    public partial class AddAnswerCountToQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswerCount",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerCount",
                table: "Questions");
        }
    }
}
