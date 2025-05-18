using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandingPage.Migrations
{
    /// <inheritdoc />
    public partial class AddSixFieldToCompanyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewImageUrlOne",
                table: "CompanyProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReviewImageUrlThree",
                table: "CompanyProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReviewImageUrlTwo",
                table: "CompanyProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReviewLongDes",
                table: "CompanyProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReviewShortDes",
                table: "CompanyProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReviewTitle",
                table: "CompanyProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewImageUrlOne",
                table: "CompanyProfile");

            migrationBuilder.DropColumn(
                name: "ReviewImageUrlThree",
                table: "CompanyProfile");

            migrationBuilder.DropColumn(
                name: "ReviewImageUrlTwo",
                table: "CompanyProfile");

            migrationBuilder.DropColumn(
                name: "ReviewLongDes",
                table: "CompanyProfile");

            migrationBuilder.DropColumn(
                name: "ReviewShortDes",
                table: "CompanyProfile");

            migrationBuilder.DropColumn(
                name: "ReviewTitle",
                table: "CompanyProfile");
        }
    }
}
