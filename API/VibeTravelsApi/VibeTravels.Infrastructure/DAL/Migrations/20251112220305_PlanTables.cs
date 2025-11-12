using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VibeTravels.Infrastructure.DAL.Migrations
{
    /// <inheritdoc />
    public partial class PlanTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Profiles",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.CreateTable(
                name: "trip_requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    NoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Days = table.Column<int>(type: "integer", nullable: false),
                    Travelers = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trip_requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_trip_requests_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trip_requests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "plan_generations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InputPayload = table.Column<string>(type: "jsonb", nullable: false),
                    OutputRaw = table.Column<string>(type: "text", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plan_generations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_plan_generations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_plan_generations_trip_requests_TripRequestId",
                        column: x => x.TripRequestId,
                        principalTable: "trip_requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "plans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TripRequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanGenerationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StructureType = table.Column<string>(type: "text", nullable: false),
                    days_count = table.Column<int>(type: "integer", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false, defaultValue: "Generated"),
                    DecisionReason = table.Column<string>(type: "text", nullable: true),
                    DecisionAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AdjustedByUser = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plans", x => x.Id);
                    table.CheckConstraint("CK_Plans_DaysCount", "days_count IS NULL OR days_count > 0");
                    table.ForeignKey(
                        name: "FK_plans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_plans_plan_generations_PlanGenerationId",
                        column: x => x.PlanGenerationId,
                        principalTable: "plan_generations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_plans_trip_requests_TripRequestId",
                        column: x => x.TripRequestId,
                        principalTable: "trip_requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_plan_generations_Id_UserId",
                table: "plan_generations",
                columns: new[] { "Id", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_plan_generations_TripRequestId",
                table: "plan_generations",
                column: "TripRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_plan_generations_UserId",
                table: "plan_generations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_plans_PlanGenerationId",
                table: "plans",
                column: "PlanGenerationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_plans_TripRequestId",
                table: "plans",
                column: "TripRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_plans_UserId",
                table: "plans",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_trip_requests_Id_UserId",
                table: "trip_requests",
                columns: new[] { "Id", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trip_requests_NoteId",
                table: "trip_requests",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_trip_requests_UserId",
                table: "trip_requests",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "plans");

            migrationBuilder.DropTable(
                name: "plan_generations");

            migrationBuilder.DropTable(
                name: "trip_requests");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Profiles",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
