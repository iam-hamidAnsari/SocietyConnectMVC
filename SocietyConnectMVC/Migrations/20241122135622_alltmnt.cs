using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocietyConnectMVC.Migrations
{
    /// <inheritdoc />
    public partial class alltmnt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flat_Alltmnt",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    allottedto = table.Column<string>(name: "allotted_to", type: "nvarchar(100)", maxLength: 100, nullable: false),
                    flatno = table.Column<string>(name: "flat_no", type: "nvarchar(100)", maxLength: 100, nullable: false),
                    floorno = table.Column<int>(name: "floor_no", type: "int", nullable: false),
                    blockno = table.Column<string>(name: "block_no", type: "nvarchar(100)", maxLength: 100, nullable: false),
                    flattype = table.Column<string>(name: "flat_type", type: "nvarchar(100)", maxLength: 100, nullable: false),
                    moveindt = table.Column<string>(name: "move_in_dt", type: "nvarchar(max)", nullable: true),
                    moveoutdt = table.Column<string>(name: "move_out_dt", type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flat_Alltmnt", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flat_Alltmnt");
        }
    }
}
