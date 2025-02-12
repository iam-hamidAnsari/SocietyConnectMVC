using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocietyConnectMVC.Migrations
{
    /// <inheritdoc />
    public partial class flats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "flats",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    flatno = table.Column<string>(name: "flat_no", type: "nvarchar(max)", nullable: false),
                    floorno = table.Column<int>(name: "floor_no", type: "int", nullable: false),
                    blockno = table.Column<string>(name: "block_no", type: "nvarchar(max)", nullable: false),
                    flattype = table.Column<string>(name: "flat_type", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_flats", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "flats");
        }
    }
}
