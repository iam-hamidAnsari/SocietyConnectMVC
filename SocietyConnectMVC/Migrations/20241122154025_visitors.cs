using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocietyConnectMVC.Migrations
{
    /// <inheritdoc />
    public partial class visitors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "visitors",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    allotedflats = table.Column<int>(name: "alloted_flats", type: "int", nullable: false),
                    visitorname = table.Column<string>(name: "visitor_name", type: "nvarchar(max)", nullable: false),
                    visitorcontact = table.Column<string>(name: "visitor_contact", type: "nvarchar(max)", nullable: false),
                    meetto = table.Column<string>(name: "meet_to", type: "nvarchar(max)", nullable: false),
                    purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    checkindt = table.Column<string>(name: "check_in_dt", type: "nvarchar(max)", nullable: false),
                    checkoutdt = table.Column<string>(name: "check_out_dt", type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visitors", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "visitors");
        }
    }
}
