using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trader.Migrations
{
    public partial class AddLinkFileImportTrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileImportId",
                table: "TradeImport",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TradeImport_FileImportId",
                table: "TradeImport",
                column: "FileImportId");

            migrationBuilder.AddForeignKey(
                name: "FK_TradeImport_FileImport_FileImportId",
                table: "TradeImport",
                column: "FileImportId",
                principalTable: "FileImport",
                principalColumn: "FileImportId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradeImport_FileImport_FileImportId",
                table: "TradeImport");

            migrationBuilder.DropIndex(
                name: "IX_TradeImport_FileImportId",
                table: "TradeImport");

            migrationBuilder.DropColumn(
                name: "FileImportId",
                table: "TradeImport");
        }
    }
}
