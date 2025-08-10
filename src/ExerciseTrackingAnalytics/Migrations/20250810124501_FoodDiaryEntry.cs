using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExerciseTrackingAnalytics.Migrations
{
    public partial class FoodDiaryEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodDiaryEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeOfDay = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Meal = table.Column<string>(type: "VARCHAR(32)", nullable: false),
                    FoodId = table.Column<long>(type: "bigint", nullable: false),
                    NumServings = table.Column<decimal>(type: "numeric", nullable: false),
                    RecordInsertDateUtc = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "now() AT TIME ZONE 'UTC'"),
                    RecordUpdateDateUtc = table.Column<DateTime>(type: "TIMESTAMP", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodDiaryEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodDiaryEntries_AspNetUsers_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodDiaryEntries_MasterFoods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "MasterFoods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodDiaryEntries_FoodId",
                table: "FoodDiaryEntries",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodDiaryEntries_OwnerUserId",
                table: "FoodDiaryEntries",
                column: "OwnerUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodDiaryEntries");
        }
    }
}
