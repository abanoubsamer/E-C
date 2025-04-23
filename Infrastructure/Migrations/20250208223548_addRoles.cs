using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(table: "AspNetRoles", 
                columns: new string[] { "Id","Name", "NormalizedName", "ConcurrencyStamp"},
                values: new object[] {Guid.NewGuid().ToString(),"User", "User".ToUpper(), Guid.NewGuid().ToString() });
            migrationBuilder.InsertData(table: "AspNetRoles",
               columns: new string[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { Guid.NewGuid().ToString(), "Admin", "Admin".ToUpper(), Guid.NewGuid().ToString() });
            migrationBuilder.InsertData(table: "AspNetRoles",
               columns: new string[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { Guid.NewGuid().ToString(), "Seller", "Seller".ToUpper(), Guid.NewGuid().ToString() });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FORM [AspNetRoles]");

        }
    }
}
