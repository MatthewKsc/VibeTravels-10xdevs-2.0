using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibeTravels.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AdjustPlanDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_plans_plan_generations_PlanGenerationId",
                table: "plans");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "plans",
                type: "text",
                nullable: false,
                defaultValue: "NotGenerated",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "Generated");

            migrationBuilder.AddForeignKey(
                name: "FK_plans_plan_generations_PlanGenerationId",
                table: "plans",
                column: "PlanGenerationId",
                principalTable: "plan_generations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_plans_plan_generations_PlanGenerationId",
                table: "plans");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "plans",
                type: "text",
                nullable: false,
                defaultValue: "Generated",
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "NotGenerated");

            migrationBuilder.AddForeignKey(
                name: "FK_plans_plan_generations_PlanGenerationId",
                table: "plans",
                column: "PlanGenerationId",
                principalTable: "plan_generations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
