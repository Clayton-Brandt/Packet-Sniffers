using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EarbudExplorer.Migrations
{
    /// <inheritdoc />
    public partial class PreventBrandCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Earbuds_Brands_BrandId",
                table: "Earbuds");

            migrationBuilder.AddForeignKey(
                name: "FK_Earbuds_Brands_BrandId",
                table: "Earbuds",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Earbuds_Brands_BrandId",
                table: "Earbuds");

            migrationBuilder.AddForeignKey(
                name: "FK_Earbuds_Brands_BrandId",
                table: "Earbuds",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
