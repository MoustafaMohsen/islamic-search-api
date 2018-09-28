using Microsoft.EntityFrameworkCore.Migrations;

namespace IslamicSearch.Migrations
{
    public partial class HadithBlocks_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "number",
                table: "HadithBlocks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "number",
                table: "HadithBlocks");
        }
    }
}
