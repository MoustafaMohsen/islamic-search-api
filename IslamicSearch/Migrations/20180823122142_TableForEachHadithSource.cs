using Microsoft.EntityFrameworkCore.Migrations;

namespace IslamicSearch.Migrations
{
    public partial class TableForEachHadithSource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HadithModel");

            migrationBuilder.AddColumn<int>(
                name: "vol",
                table: "In_Book_Refrence",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BukhariHadith",
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
                    table.PrimaryKey("PK_BukhariHadith", x => x.id);
                    table.ForeignKey(
                        name: "FK_BukhariHadith_In_Book_Refrence_in_book_refrenceid",
                        column: x => x.in_book_refrenceid,
                        principalTable: "In_Book_Refrence",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BukhariHadith_Old_refrence_old_refrenceid",
                        column: x => x.old_refrenceid,
                        principalTable: "Old_refrence",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MuslimHadith",
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
                    table.PrimaryKey("PK_MuslimHadith", x => x.id);
                    table.ForeignKey(
                        name: "FK_MuslimHadith_In_Book_Refrence_in_book_refrenceid",
                        column: x => x.in_book_refrenceid,
                        principalTable: "In_Book_Refrence",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MuslimHadith_Old_refrence_old_refrenceid",
                        column: x => x.old_refrenceid,
                        principalTable: "Old_refrence",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NasaiHadith",
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
                    table.PrimaryKey("PK_NasaiHadith", x => x.id);
                    table.ForeignKey(
                        name: "FK_NasaiHadith_In_Book_Refrence_in_book_refrenceid",
                        column: x => x.in_book_refrenceid,
                        principalTable: "In_Book_Refrence",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NasaiHadith_Old_refrence_old_refrenceid",
                        column: x => x.old_refrenceid,
                        principalTable: "Old_refrence",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BukhariHadith_in_book_refrenceid",
                table: "BukhariHadith",
                column: "in_book_refrenceid");

            migrationBuilder.CreateIndex(
                name: "IX_BukhariHadith_old_refrenceid",
                table: "BukhariHadith",
                column: "old_refrenceid");

            migrationBuilder.CreateIndex(
                name: "IX_MuslimHadith_in_book_refrenceid",
                table: "MuslimHadith",
                column: "in_book_refrenceid");

            migrationBuilder.CreateIndex(
                name: "IX_MuslimHadith_old_refrenceid",
                table: "MuslimHadith",
                column: "old_refrenceid");

            migrationBuilder.CreateIndex(
                name: "IX_NasaiHadith_in_book_refrenceid",
                table: "NasaiHadith",
                column: "in_book_refrenceid");

            migrationBuilder.CreateIndex(
                name: "IX_NasaiHadith_old_refrenceid",
                table: "NasaiHadith",
                column: "old_refrenceid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BukhariHadith");

            migrationBuilder.DropTable(
                name: "MuslimHadith");

            migrationBuilder.DropTable(
                name: "NasaiHadith");

            migrationBuilder.DropColumn(
                name: "vol",
                table: "In_Book_Refrence");

            migrationBuilder.CreateTable(
                name: "HadithModel",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    arabicHTML = table.Column<string>(nullable: true),
                    arabicText = table.Column<string>(nullable: true),
                    englishHTML = table.Column<string>(nullable: true),
                    englishText = table.Column<string>(nullable: true),
                    in_book_refrenceid = table.Column<int>(nullable: true),
                    number = table.Column<int>(nullable: false),
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
    }
}
