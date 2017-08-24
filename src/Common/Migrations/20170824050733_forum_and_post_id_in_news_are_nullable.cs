using Microsoft.EntityFrameworkCore.Migrations;

namespace BioEngine.Common.Migrations
{
    public partial class forum_and_post_id_in_news_are_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "pid",
                table: "be_news",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "pid",
                table: "be_news",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
