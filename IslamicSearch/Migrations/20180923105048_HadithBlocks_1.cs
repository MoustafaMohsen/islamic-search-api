using Microsoft.EntityFrameworkCore.Migrations;

namespace IslamicSearch.Migrations
{
    public partial class HadithBlocks_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HadithBlocks",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    src = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HadithBlocks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Refrences",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true),
                    Refrencetype = table.Column<string>(nullable: true),
                    value1 = table.Column<int>(nullable: false),
                    value2 = table.Column<int>(nullable: false),
                    value3 = table.Column<int>(nullable: false),
                    value4 = table.Column<int>(nullable: false),
                    tag1 = table.Column<string>(nullable: true),
                    tag2 = table.Column<string>(nullable: true),
                    HadithBlocksid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refrences", x => x.id);
                    table.ForeignKey(
                        name: "FK_Refrences_HadithBlocks_HadithBlocksid",
                        column: x => x.HadithBlocksid,
                        principalTable: "HadithBlocks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Value",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true),
                    value = table.Column<string>(nullable: true),
                    HadithBlocksid = table.Column<int>(nullable: true),
                    HadithBlocksid1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Value", x => x.id);
                    table.ForeignKey(
                        name: "FK_Value_HadithBlocks_HadithBlocksid",
                        column: x => x.HadithBlocksid,
                        principalTable: "HadithBlocks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Value_HadithBlocks_HadithBlocksid1",
                        column: x => x.HadithBlocksid1,
                        principalTable: "HadithBlocks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Refrences_HadithBlocksid",
                table: "Refrences",
                column: "HadithBlocksid");

            migrationBuilder.CreateIndex(
                name: "IX_Value_HadithBlocksid",
                table: "Value",
                column: "HadithBlocksid");

            migrationBuilder.CreateIndex(
                name: "IX_Value_HadithBlocksid1",
                table: "Value",
                column: "HadithBlocksid1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Refrences");

            migrationBuilder.DropTable(
                name: "Value");

            migrationBuilder.DropTable(
                name: "HadithBlocks");
        }
    }
}
