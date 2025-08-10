using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ExerciseTrackingAnalytics.Migrations
{
    public partial class MasterFood : Migration
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
                    NameNormalized = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    BrandName = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ServingSize = table.Column<decimal>(type: "numeric", nullable: false),
                    ServingSizeUnit = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    RecordInsertDateUtc = table.Column<DateTime>(type: "TIMESTAMP", nullable: false, defaultValueSql: "now() AT TIME ZONE 'UTC'"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsShared = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DeactivatedDateUtc = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    DeactivatedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    PredecessorId = table.Column<long>(type: "bigint", nullable: true),
                    Calories = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalFatGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    SaturatedFatGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    PolyUnsaturatedFatGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    MonoUnsaturatedFatGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    TransFatGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    CholesterolMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    SodiumMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    PotassiumMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    TotalCarbohydratesGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    DietaryFiberGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    TotalSugarsGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    ProteinGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    VitaminA_MicroGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    VitaminC_MicroGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    VitaminD_MicroGrams = table.Column<decimal>(type: "numeric", nullable: true),
                    CalciumMilligrams = table.Column<decimal>(type: "numeric", nullable: true),
                    IronMilligrams = table.Column<decimal>(type: "numeric", nullable: true)
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
                unique: true);

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
                name: "MasterFoods");
        }
    }
}
