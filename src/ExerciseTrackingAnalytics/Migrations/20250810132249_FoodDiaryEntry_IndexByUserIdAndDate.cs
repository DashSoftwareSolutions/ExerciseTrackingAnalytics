using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExerciseTrackingAnalytics.Migrations
{
    public partial class FoodDiaryEntry_IndexByUserIdAndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FoodDiaryEntries_OwnerUserId",
                table: "FoodDiaryEntries");

            migrationBuilder.CreateIndex(
                name: "IX_FoodDiaryEntries_OwnerUserId_Date",
                table: "FoodDiaryEntries",
                columns: new[] { "OwnerUserId", "Date" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FoodDiaryEntries_OwnerUserId_Date",
                table: "FoodDiaryEntries");

            migrationBuilder.CreateIndex(
                name: "IX_FoodDiaryEntries_OwnerUserId",
                table: "FoodDiaryEntries",
                column: "OwnerUserId");
        }
    }
}
