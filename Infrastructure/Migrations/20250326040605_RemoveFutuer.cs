using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFutuer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureCompatibilitys");

            migrationBuilder.DropTable(
                name: "ModelFeatures");

            migrationBuilder.DropTable(
                name: "FeatureOptions");

            migrationBuilder.DropTable(
                name: "Features");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeatureOptions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "NEWID()"),
                    FeatureId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureOptions_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeatureCompatibilitys",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "NEWID()"),
                    FeatureOptionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCompatibilitys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureCompatibilitys_FeatureOptions_FeatureOptionId",
                        column: x => x.FeatureOptionId,
                        principalTable: "FeatureOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureCompatibilitys_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModelFeatures",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "NEWID()"),
                    FeatureOptionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelFeatures_FeatureOptions_FeatureOptionId",
                        column: x => x.FeatureOptionId,
                        principalTable: "FeatureOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelFeatures_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCompatibilitys_FeatureOptionId",
                table: "FeatureCompatibilitys",
                column: "FeatureOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCompatibilitys_ProductId",
                table: "FeatureCompatibilitys",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureOptions_FeatureId",
                table: "FeatureOptions",
                column: "FeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelFeatures_FeatureOptionId",
                table: "ModelFeatures",
                column: "FeatureOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelFeatures_ModelId",
                table: "ModelFeatures",
                column: "ModelId");
        }
    }
}
