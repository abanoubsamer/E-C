using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoenUserInOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PhoneId",
                table: "Orders",
                column: "PhoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_UserPhoneNumbers_PhoneId",
                table: "Orders",
                column: "PhoneId",
                principalTable: "UserPhoneNumbers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_UserPhoneNumbers_PhoneId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PhoneId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PhoneId",
                table: "Orders");
        }
    }
}
