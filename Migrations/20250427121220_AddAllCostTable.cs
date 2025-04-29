using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandingPage.Migrations
{
    /// <inheritdoc />
    public partial class AddAllCostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingFees",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ShippingFees",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingFees",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "ShippingFees",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
