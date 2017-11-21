using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TraderData.Migrations
{
    public partial class ChangeEnquiries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Enquiry");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Enquiry",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Enquiry_UserID",
                table: "Enquiry",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Enquiry_AspNetUsers_UserID",
                table: "Enquiry",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enquiry_AspNetUsers_UserID",
                table: "Enquiry");

            migrationBuilder.DropIndex(
                name: "IX_Enquiry_UserID",
                table: "Enquiry");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Enquiry");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Enquiry",
                maxLength: 128,
                nullable: true);
        }
    }
}
