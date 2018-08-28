using Microsoft.EntityFrameworkCore.Migrations;

namespace IslamicSearch.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "In_Book_Refrence",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    book = table.Column<int>(nullable: false),
                    hadith = table.Column<int>(nullable: false),
                    tag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_In_Book_Refrence", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Old_refrence",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    vol = table.Column<int>(nullable: false),
                    book = table.Column<int>(nullable: false),
                    hadith = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Old_refrence", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "HadithModel",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    number = table.Column<int>(nullable: false),
                    arabicHTML = table.Column<string>(nullable: true),
                    arabicText = table.Column<string>(nullable: true),
                    englishHTML = table.Column<string>(nullable: true),
                    englishText = table.Column<string>(nullable: true),
                    in_book_refrenceid = table.Column<int>(nullable: true),
                    old_refrenceid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HadithModel", x => x.id);
                    table.ForeignKey(
                        name: "FK_HadithModel_In_Book_Refrence_in_book_refrenceid",
                        column: x => x.in_book_refrenceid,
                        principalTable: "In_Book_Refrence",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HadithModel_Old_refrence_old_refrenceid",
                        column: x => x.old_refrenceid,
                        principalTable: "Old_refrence",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HadithModel_in_book_refrenceid",
                table: "HadithModel",
                column: "in_book_refrenceid");

            migrationBuilder.CreateIndex(
                name: "IX_HadithModel_old_refrenceid",
                table: "HadithModel",
                column: "old_refrenceid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HadithModel");

            migrationBuilder.DropTable(
                name: "In_Book_Refrence");

            migrationBuilder.DropTable(
                name: "Old_refrence");
        }
    }
}
