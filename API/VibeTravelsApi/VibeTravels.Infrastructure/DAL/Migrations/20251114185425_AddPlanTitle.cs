using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibeTravels.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "plans",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "plan_generations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "plans");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "plan_generations");
        }
    }
}
