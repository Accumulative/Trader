using Microsoft.EntityFrameworkCore.Migrations;

namespace TraderData.Migrations
{
    public partial class AddUserToModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "TradeImport",
                nullable: false,
                defaultValue: "f98bf459-15d5-441a-adfc-100ad82f7629");

            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "FileImport",
                nullable: false,
                defaultValue: "f98bf459-15d5-441a-adfc-100ad82f7629");

            migrationBuilder.CreateIndex(
                name: "IX_TradeImport_UserID",
                table: "TradeImport",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_FileImport_UserID",
                table: "FileImport",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_FileImport_AspNetUsers_UserID",
                table: "FileImport",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeImport_AspNetUsers_UserID",
                table: "TradeImport",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileImport_AspNetUsers_UserID",
                table: "FileImport");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeImport_AspNetUsers_UserID",
                table: "TradeImport");

            migrationBuilder.DropIndex(
                name: "IX_TradeImport_UserID",
                table: "TradeImport");

            migrationBuilder.DropIndex(
                name: "IX_FileImport_UserID",
                table: "FileImport");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "TradeImport");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "FileImport");
        }
    }
}
