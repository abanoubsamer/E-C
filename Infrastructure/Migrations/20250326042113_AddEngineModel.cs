using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEngineModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelCompatibilitys_Models_ModelId",
                table: "ModelCompatibilitys");

            migrationBuilder.RenameColumn(
                name: "ModelId",
                table: "ModelCompatibilitys",
                newName: "ModelEngineId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelCompatibilitys_ModelId",
                table: "ModelCompatibilitys",
                newName: "IX_ModelCompatibilitys_ModelEngineId");

            migrationBuilder.CreateTable(
                name: "EngineType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelEngine",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EngineTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModelId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelEngine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelEngine_EngineType_EngineTypeId",
                        column: x => x.EngineTypeId,
                        principalTable: "EngineType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelEngine_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelEngine_EngineTypeId",
                table: "ModelEngine",
                column: "EngineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelEngine_ModelId",
                table: "ModelEngine",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelCompatibilitys_ModelEngine_ModelEngineId",
                table: "ModelCompatibilitys",
                column: "ModelEngineId",
                principalTable: "ModelEngine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModelCompatibilitys_ModelEngine_ModelEngineId",
                table: "ModelCompatibilitys");

            migrationBuilder.DropTable(
                name: "ModelEngine");

            migrationBuilder.DropTable(
                name: "EngineType");

            migrationBuilder.RenameColumn(
                name: "ModelEngineId",
                table: "ModelCompatibilitys",
                newName: "ModelId");

            migrationBuilder.RenameIndex(
                name: "IX_ModelCompatibilitys_ModelEngineId",
                table: "ModelCompatibilitys",
                newName: "IX_ModelCompatibilitys_ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModelCompatibilitys_Models_ModelId",
                table: "ModelCompatibilitys",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
