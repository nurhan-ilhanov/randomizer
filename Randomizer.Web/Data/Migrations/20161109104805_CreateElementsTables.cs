using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Randomizer.Web.Data.Migrations
{
    public partial class CreateElementsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElementLists",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementLists", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Elements",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ElementListID = table.Column<int>(nullable: false),
                    ModifiedBy = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    UserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Elements_ElementLists_ElementListID",
                        column: x => x.ElementListID,
                        principalTable: "ElementLists",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Elements_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Elements_ElementListID",
                table: "Elements",
                column: "ElementListID");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_UserID",
                table: "Elements",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Elements");

            migrationBuilder.DropTable(
                name: "ElementLists");
        }
    }
}
