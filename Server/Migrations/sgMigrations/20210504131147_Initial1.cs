using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Server.Migrations.sgMigrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Codes",
                columns: table => new
                {
                    id_code = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Codes", x => x.id_code);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    id_device = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    number = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.id_device);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    id_group = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.id_group);
                });

            migrationBuilder.CreateTable(
                name: "Journal",
                columns: table => new
                {
                    id_surgard = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_action = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    id_code = table.Column<int>(type: "integer", nullable: false),
                    id_device = table.Column<int>(type: "integer", nullable: false),
                    id_group = table.Column<int>(type: "integer", nullable: false),
                    di_groupsid_group = table.Column<int>(type: "integer", nullable: true),
                    di_codesid_code = table.Column<int>(type: "integer", nullable: true),
                    di_devicesid_device = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journal", x => x.id_surgard);
                    table.ForeignKey(
                        name: "FK_Journal_Codes_di_codesid_code",
                        column: x => x.di_codesid_code,
                        principalTable: "Codes",
                        principalColumn: "id_code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Journal_Devices_di_devicesid_device",
                        column: x => x.di_devicesid_device,
                        principalTable: "Devices",
                        principalColumn: "id_device",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Journal_Groups_di_groupsid_group",
                        column: x => x.di_groupsid_group,
                        principalTable: "Groups",
                        principalColumn: "id_group",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Codes",
                columns: new[] { "id_code", "code", "name" },
                values: new object[,]
                {
                    { 1, "303", "Линейная алгебра" },
                    { 2, "403", "Нелинейный русский язык" },
                    { 3, "703", "Арифметическая география" }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "id_device", "name", "number" },
                values: new object[,]
                {
                    { 1, "Дмитрий", "01" },
                    { 2, "Вафлий", "02" },
                    { 3, "Печений", "03" }
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "id_group", "code", "name" },
                values: new object[,]
                {
                    { 1, "01", "Г01" },
                    { 2, "02", "Г02" },
                    { 3, "99", "Г03" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Journal_di_codesid_code",
                table: "Journal",
                column: "di_codesid_code");

            migrationBuilder.CreateIndex(
                name: "IX_Journal_di_devicesid_device",
                table: "Journal",
                column: "di_devicesid_device");

            migrationBuilder.CreateIndex(
                name: "IX_Journal_di_groupsid_group",
                table: "Journal",
                column: "di_groupsid_group");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Journal");

            migrationBuilder.DropTable(
                name: "Codes");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
