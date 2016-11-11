using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Randomizer.Web.Data.Migrations
{
    public partial class AddUserID_ToElementListsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "ElementLists",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ElementLists_UserID",
                table: "ElementLists",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementLists_AspNetUsers_UserID",
                table: "ElementLists",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementLists_AspNetUsers_UserID",
                table: "ElementLists");

            migrationBuilder.DropIndex(
                name: "IX_ElementLists_UserID",
                table: "ElementLists");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "ElementLists");
        }
    }
}
