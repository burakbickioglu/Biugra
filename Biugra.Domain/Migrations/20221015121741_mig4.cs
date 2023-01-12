using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biugra.Domain.Migrations
{
    public partial class mig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Vallets_ValletId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ValletId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Vallets");

            migrationBuilder.DropColumn(
                name: "ValletId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Vallets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Vallets_AppUserId",
                table: "Vallets",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vallets_AspNetUsers_AppUserId",
                table: "Vallets",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vallets_AspNetUsers_AppUserId",
                table: "Vallets");

            migrationBuilder.DropIndex(
                name: "IX_Vallets_AppUserId",
                table: "Vallets");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Vallets");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Vallets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ValletId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ValletId",
                table: "AspNetUsers",
                column: "ValletId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Vallets_ValletId",
                table: "AspNetUsers",
                column: "ValletId",
                principalTable: "Vallets",
                principalColumn: "Id");
        }
    }
}
