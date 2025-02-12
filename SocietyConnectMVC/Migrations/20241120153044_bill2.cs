using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocietyConnectMVC.Migrations
{
    /// <inheritdoc />
    public partial class bill2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Paid_Amount",
                table: "bills",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pymnt_type",
                table: "bills",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid_Amount",
                table: "bills");

            migrationBuilder.DropColumn(
                name: "Pymnt_type",
                table: "bills");
        }
    }
}
