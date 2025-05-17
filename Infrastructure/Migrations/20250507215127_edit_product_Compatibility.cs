using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class edit_product_Compatibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelCompatibilitys_Products_ProductId",
                table: "ModelCompatibilitys");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ModelCompatibilitys",
                newName: "SKU");

            migrationBuilder.RenameIndex(
                name: "IX_ModelCompatibilitys_ProductId",
                table: "ModelCompatibilitys",
                newName: "IX_ModelCompatibilitys_SKU");

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelCompatibilitys_Products_SKU",
                table: "ModelCompatibilitys",
                column: "SKU",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelCompatibilitys_Products_SKU",
                table: "ModelCompatibilitys");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "SKU",
                table: "ModelCompatibilitys",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelCompatibilitys_SKU",
                table: "ModelCompatibilitys",
                newName: "IX_ModelCompatibilitys_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelCompatibilitys_Products_ProductId",
                table: "ModelCompatibilitys",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
