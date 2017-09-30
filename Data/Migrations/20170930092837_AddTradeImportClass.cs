using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Trader.Data.Migrations
{
    public partial class AddTradeImportClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradeImport",
                columns: table => new
                {
                    TradeImportID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExternalReference = table.Column<string>(nullable: false),
                    ImportDate = table.Column<DateTime>(nullable: false),
                    InstrumentModelID = table.Column<int>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    Value = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeImport", x => x.TradeImportID);
                    table.ForeignKey(
                        name: "FK_TradeImport_InstrumentModel_InstrumentModelID",
                        column: x => x.InstrumentModelID,
                        principalTable: "InstrumentModel",
                        principalColumn: "InstrumentModelID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradeImport_InstrumentModelID",
                table: "TradeImport",
                column: "InstrumentModelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeImport");
        }
    }
}
