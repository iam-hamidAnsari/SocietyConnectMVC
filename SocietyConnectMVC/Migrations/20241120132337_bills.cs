using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocietyConnectMVC.Migrations
{
    /// <inheritdoc />
    public partial class bills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlatNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    useremail = table.Column<string>(name: "user_email", type: "nvarchar(max)", nullable: true),
                    BillTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillPdf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bills", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bills");
        }
    }
}
