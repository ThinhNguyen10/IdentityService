using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Migrations
{
    public partial class V0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ACCOUNT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    USERNAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PASSWORD = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FULLNAME = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PHONE = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCOUNT", x => x.ID);
                    table.UniqueConstraint("AK_ACCOUNT_USERNAME", x => x.USERNAME);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACCOUNT");
        }
    }
}
