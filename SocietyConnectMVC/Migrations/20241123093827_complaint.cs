using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocietyConnectMVC.Migrations
{
    /// <inheritdoc />
    public partial class complaint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserEmail = table.Column<string>(name: "User_Email", type: "nvarchar(max)", nullable: true),
                    AllotedFlat = table.Column<int>(type: "int", nullable: true),
                    ComplaintType = table.Column<string>(name: "Complaint_Type", type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolvedAt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    comments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Complaints");
        }
    }
}
