using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace BioEngine.Data.DB.Migrations
{
    public partial class pid_can_be_null_in_GalleryCat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "pid",
                table: "be_gallery_cats",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.Sql("UPDATE `be_gallery_cats` SET `pid`=null WHERE `pid` = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_be_gallery_cats_be_gallery_cats_pid",
                table: "be_gallery_cats",
                column: "pid",
                principalTable: "be_gallery_cats",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_be_gallery_cats_be_gallery_cats_pid",
                table: "be_gallery_cats");

            migrationBuilder.AlterColumn<int>(
                name: "pid",
                table: "be_gallery_cats",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.Sql("UPDATE `be_gallery_cats` SET `pid`=0 WHERE `pid` == null");
        }
    }
}
