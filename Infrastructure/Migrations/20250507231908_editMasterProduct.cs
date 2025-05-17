using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editMasterProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelCompatibilitys_Products_SKU",
                table: "ModelCompatibilitys");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.DropTable(
                name: "EngineType");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Products_SKU",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SKU",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductsMaster",
                columns: table => new
                {
                    SKU = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsMaster", x => x.SKU);
                    table.ForeignKey(
                        name: "FK_ProductsMaster_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                table: "Products",
                column: "SKU");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsMaster_CategoryID",
                table: "ProductsMaster",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelCompatibilitys_ProductsMaster_SKU",
                table: "ModelCompatibilitys",
                column: "SKU",
                principalTable: "ProductsMaster",
                principalColumn: "SKU",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductsMaster_SKU",
                table: "Products",
                column: "SKU",
                principalTable: "ProductsMaster",
                principalColumn: "SKU",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelCompatibilitys_ProductsMaster_SKU",
                table: "ModelCompatibilitys");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductsMaster_SKU",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ProductsMaster");

            migrationBuilder.DropIndex(
                name: "IX_Products_SKU",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "CategoryID",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Products_SKU",
                table: "Products",
                column: "SKU");

            migrationBuilder.CreateTable(
                name: "EngineType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                table: "Products",
                column: "SKU",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ModelCompatibilitys_Products_SKU",
                table: "ModelCompatibilitys",
                column: "SKU",
                principalTable: "Products",
                principalColumn: "SKU",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "CategoryID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
