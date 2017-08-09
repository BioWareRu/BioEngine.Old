using Microsoft.EntityFrameworkCore.Migrations;

namespace BioEngine.Common.Migrations
{
    public partial class child_properties_can_be_null : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AlterColumn<int>(
                name: "topic_id",
                table: "be_news",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_news` SET `topic_id`=null WHERE `topic_id` = 0");
            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_news",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_news` SET `game_id`=null WHERE `game_id` = 0");
            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_news",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_news` SET `developer_id`=null WHERE `developer_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_gallery",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_gallery` SET `game_id`=null WHERE `game_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_gallery",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_gallery` SET `developer_id`=null WHERE `developer_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_gallery_cats",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_gallery_cats` SET `game_id`=null WHERE `game_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_gallery_cats",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_gallery_cats` SET `developer_id`=null WHERE `developer_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_files_cats",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_files_cats` SET `game_id`=null WHERE `game_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_files_cats",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_files_cats` SET `developer_id`=null WHERE `developer_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_files",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_files` SET `game_id`=null WHERE `game_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_files",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_files` SET `developer_id`=null WHERE `developer_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "topic_id",
                table: "be_articles_cats",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_articles_cats` SET `topic_id`=null WHERE `topic_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_articles_cats",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_articles_cats` SET `game_id`=null WHERE `game_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_articles_cats",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_articles_cats` SET `developer_id`=null WHERE `developer_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "topic_id",
                table: "be_articles",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_articles` SET `topic_id`=null WHERE `topic_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_articles",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_articles` SET `game_id`=null WHERE `game_id` = 0");

            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_articles",
                nullable: true,
                oldClrType: typeof(int));
            migrationBuilder.Sql("UPDATE `be_articles` SET `developer_id`=null WHERE `developer_id` = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_be_articles_be_developers_developer_id",
                table: "be_articles",
                column: "developer_id",
                principalTable: "be_developers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_articles_be_games_game_id",
                table: "be_articles",
                column: "game_id",
                principalTable: "be_games",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_articles_be_nuke_topics_topic_id",
                table: "be_articles",
                column: "topic_id",
                principalTable: "be_nuke_topics",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_articles_cats_be_developers_developer_id",
                table: "be_articles_cats",
                column: "developer_id",
                principalTable: "be_developers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_articles_cats_be_games_game_id",
                table: "be_articles_cats",
                column: "game_id",
                principalTable: "be_games",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_articles_cats_be_nuke_topics_topic_id",
                table: "be_articles_cats",
                column: "topic_id",
                principalTable: "be_nuke_topics",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_files_be_developers_developer_id",
                table: "be_files",
                column: "developer_id",
                principalTable: "be_developers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_files_be_games_game_id",
                table: "be_files",
                column: "game_id",
                principalTable: "be_games",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_files_cats_be_developers_developer_id",
                table: "be_files_cats",
                column: "developer_id",
                principalTable: "be_developers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_files_cats_be_games_game_id",
                table: "be_files_cats",
                column: "game_id",
                principalTable: "be_games",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_gallery_cats_be_developers_developer_id",
                table: "be_gallery_cats",
                column: "developer_id",
                principalTable: "be_developers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_gallery_cats_be_games_game_id",
                table: "be_gallery_cats",
                column: "game_id",
                principalTable: "be_games",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_gallery_be_developers_developer_id",
                table: "be_gallery",
                column: "developer_id",
                principalTable: "be_developers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_gallery_be_games_game_id",
                table: "be_gallery",
                column: "game_id",
                principalTable: "be_games",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_news_be_developers_developer_id",
                table: "be_news",
                column: "developer_id",
                principalTable: "be_developers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_news_be_games_game_id",
                table: "be_news",
                column: "game_id",
                principalTable: "be_games",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_be_news_be_nuke_topics_topic_id",
                table: "be_news",
                column: "topic_id",
                principalTable: "be_nuke_topics",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_be_articles_be_developers_developer_id",
                table: "be_articles");

            migrationBuilder.DropForeignKey(
                name: "FK_be_articles_be_games_game_id",
                table: "be_articles");

            migrationBuilder.DropForeignKey(
                name: "FK_be_articles_be_nuke_topics_topic_id",
                table: "be_articles");

            migrationBuilder.DropForeignKey(
                name: "FK_be_articles_cats_be_developers_developer_id",
                table: "be_articles_cats");

            migrationBuilder.DropForeignKey(
                name: "FK_be_articles_cats_be_games_game_id",
                table: "be_articles_cats");

            migrationBuilder.DropForeignKey(
                name: "FK_be_articles_cats_be_nuke_topics_topic_id",
                table: "be_articles_cats");

            migrationBuilder.DropForeignKey(
                name: "FK_be_files_be_developers_developer_id",
                table: "be_files");

            migrationBuilder.DropForeignKey(
                name: "FK_be_files_be_games_game_id",
                table: "be_files");

            migrationBuilder.DropForeignKey(
                name: "FK_be_files_cats_be_developers_developer_id",
                table: "be_files_cats");

            migrationBuilder.DropForeignKey(
                name: "FK_be_files_cats_be_games_game_id",
                table: "be_files_cats");

            migrationBuilder.DropForeignKey(
                name: "FK_be_gallery_cats_be_developers_developer_id",
                table: "be_gallery_cats");

            migrationBuilder.DropForeignKey(
                name: "FK_be_gallery_cats_be_games_game_id",
                table: "be_gallery_cats");

            migrationBuilder.DropForeignKey(
                name: "FK_be_gallery_be_developers_developer_id",
                table: "be_gallery");

            migrationBuilder.DropForeignKey(
                name: "FK_be_gallery_be_games_game_id",
                table: "be_gallery");

            migrationBuilder.DropForeignKey(
                name: "FK_be_news_be_developers_developer_id",
                table: "be_news");

            migrationBuilder.DropForeignKey(
                name: "FK_be_news_be_games_game_id",
                table: "be_news");

            migrationBuilder.DropForeignKey(
                name: "FK_be_news_be_nuke_topics_topic_id",
                table: "be_news");

            migrationBuilder.AlterColumn<int>(
                name: "topic_id",
                table: "be_news",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_news",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_news",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_gallery",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_gallery",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_gallery_cats",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_gallery_cats",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_files_cats",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_files_cats",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_files",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_files",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "topic_id",
                table: "be_articles_cats",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_articles_cats",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_articles_cats",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "topic_id",
                table: "be_articles",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "game_id",
                table: "be_articles",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "developer_id",
                table: "be_articles",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}