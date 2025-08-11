using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExerciseTrackingAnalytics.Migrations
{
    public partial class SchemaForFoodDiary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterFoods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    NameNormalized = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    BrandName = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ServingSize = table.Column<decimal>(type: "numeric", nullable: false),
                    ServingSizeUnit = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Calories = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalFatGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    SaturatedFatGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    PolyUnsaturatedFatGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    MonoUnsaturatedFatGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    TransFatGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    CholesterolMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    SodiumMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    TotalCarbohydratesGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    DietaryFiberGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    TotalSugarsGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    AddedSugarsGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    ProteinGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    VitaminA_Micrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    VitaminB6_Micrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    VitaminB12_Micrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    VitaminC_Micrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    VitaminD_Micrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    VitaminE_Micrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    VitaminK_Micrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    BiotinMicrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    CholineMicrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    FolateMicrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    NiacinMicrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    PantothenicAcidMicrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    RiboflavinMicrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    ThiaminMicrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    CalciumMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    ChlorideMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    ChromiumMicrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    CopperMicrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    IodineMicrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    IronMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    MagnesiumMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    ManganeseMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    MolybdenumMicrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    PhosphorusMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    PotassiumMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    SeleniumMicrograms = table.Column<decimal>(type: "numeric", nullable: true),
                    ZincMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    RecordInsertDateUtc = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "now() AT TIME ZONE 'UTC'"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsShared = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Barcode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    BarcodeNormalized = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    DeactivatedDateUtc = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    DeactivatedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    PredecessorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterFoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterFoods_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterFoods_AspNetUsers_DeactivatedByUserId",
                        column: x => x.DeactivatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MasterFoods_AspNetUsers_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MasterFoods_MasterFoods_PredecessorId",
                        column: x => x.PredecessorId,
                        principalTable: "MasterFoods",
                        principalColumn: "Id");
                });

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
                name: "IX_FoodDiaryEntries_OwnerUserId_Date",
                table: "FoodDiaryEntries",
                columns: new[] { "OwnerUserId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_MasterFoods_BarcodeNormalized",
                table: "MasterFoods",
                column: "BarcodeNormalized");

            migrationBuilder.CreateIndex(
                name: "IX_MasterFoods_CreatedByUserId",
                table: "MasterFoods",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterFoods_DeactivatedByUserId",
                table: "MasterFoods",
                column: "DeactivatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterFoods_NameNormalized_Version",
                table: "MasterFoods",
                columns: new[] { "NameNormalized", "Version" },
                unique: true,
                filter: "\"OwnerUserId\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MasterFoods_NameNormalized_Version_OwnerUserId",
                table: "MasterFoods",
                columns: new[] { "NameNormalized", "Version", "OwnerUserId" },
                unique: true,
                filter: "\"OwnerUserId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MasterFoods_OwnerUserId",
                table: "MasterFoods",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterFoods_PredecessorId",
                table: "MasterFoods",
                column: "PredecessorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodDiaryEntries");

            migrationBuilder.DropTable(
                name: "MasterFoods");
        }
    }
}
