using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocietyConnectMVC.Migrations
{
    /// <inheritdoc />
    public partial class isalloted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "is_allotted",
                table: "flats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_allotted",
                table: "flats");
        }
    }
}
