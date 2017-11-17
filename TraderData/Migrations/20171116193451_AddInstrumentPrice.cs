using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TraderData.Migrations
{
    public partial class AddInstrumentPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstrumentPrice",
                columns: table => new
                {
                    InstrumentPriceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateTime = table.Column<DateTime>(nullable: false),
                    ExchangeID = table.Column<int>(nullable: false),
                    InstrumentID = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentPrice", x => x.InstrumentPriceID);
                    table.ForeignKey(
                        name: "FK_InstrumentPrice_Exchange_ExchangeID",
                        column: x => x.ExchangeID,
                        principalTable: "Exchange",
                        principalColumn: "ExchangeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstrumentPrice_Instrument_InstrumentID",
                        column: x => x.InstrumentID,
                        principalTable: "Instrument",
                        principalColumn: "InstrumentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentPrice_ExchangeID",
                table: "InstrumentPrice",
                column: "ExchangeID");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentPrice_InstrumentID",
                table: "InstrumentPrice",
                column: "InstrumentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstrumentPrice");
        }
    }
}
