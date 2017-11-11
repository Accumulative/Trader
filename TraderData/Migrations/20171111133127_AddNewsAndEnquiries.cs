using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TraderData.Migrations
{
    public partial class AddNewsAndEnquiries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enquiry",
                columns: table => new
                {
                    EnquiryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<string>(maxLength: 128, nullable: true),
                    QueryType = table.Column<int>(nullable: false),
                    Ts = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enquiry", x => x.EnquiryID);
                });

            migrationBuilder.CreateTable(
                name: "NewsItem",
                columns: table => new
                {
                    NewsItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Ts = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsItem", x => x.NewsItemID);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: false),
                    EnquiryID = table.Column<int>(nullable: false),
                    FromCustomer = table.Column<bool>(nullable: false),
                    Ts = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_Message_Enquiry_EnquiryID",
                        column: x => x.EnquiryID,
                        principalTable: "Enquiry",
                        principalColumn: "EnquiryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_EnquiryID",
                table: "Message",
                column: "EnquiryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "NewsItem");

            migrationBuilder.DropTable(
                name: "Enquiry");
        }
    }
}
