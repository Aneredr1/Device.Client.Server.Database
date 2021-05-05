using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Migrations.UserMigrations
{
    public partial class initusers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "User_Id", "Login", "Password", "User_Name" },
                values: new object[] { 1, "Login1", "password", "Арбузов Арбуз Арбузович" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "User_Id", "Login", "Password", "User_Name" },
                values: new object[] { 2, "Login2", "qwerty", "Дынев Дынь Дыньевич" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "User_Id", "Login", "Password", "User_Name" },
                values: new object[] { 3, "Login3", "podsolnuh", "Кивиев Кивь Кивиевич" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
