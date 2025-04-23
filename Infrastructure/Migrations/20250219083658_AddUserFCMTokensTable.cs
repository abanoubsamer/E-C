using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserFCMTokensTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Card_UserID",
                table: "Card");

            migrationBuilder.CreateTable(
                name: "UserFCMTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "NEWID()"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFCMTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFCMTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Card_UserID",
                table: "Card",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFCMTokens_UserId",
                table: "UserFCMTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFCMTokens");

            migrationBuilder.DropIndex(
                name: "IX_Card_UserID",
                table: "Card");

            migrationBuilder.CreateIndex(
                name: "IX_Card_UserID",
                table: "Card",
                column: "UserID");
        }
    }
}
