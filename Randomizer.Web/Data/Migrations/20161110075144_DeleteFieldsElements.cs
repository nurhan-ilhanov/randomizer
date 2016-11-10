using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Randomizer.Web.Data.Migrations
{
    public partial class DeleteFieldsElements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Elements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Elements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "Elements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Elements",
                nullable: false,
                defaultValue: 0);
        }
    }
}
