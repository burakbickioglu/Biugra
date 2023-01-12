using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biugra.Domain.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Vallets_ValletId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "ValletId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Vallets_ValletId",
                table: "AspNetUsers",
                column: "ValletId",
                principalTable: "Vallets",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Vallets_ValletId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "ValletId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Vallets_ValletId",
                table: "AspNetUsers",
                column: "ValletId",
                principalTable: "Vallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
