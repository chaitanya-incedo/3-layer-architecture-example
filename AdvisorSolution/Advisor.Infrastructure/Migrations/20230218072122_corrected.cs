using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Advisor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class corrected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvisorClients_Users_UsersUserID",
                table: "AdvisorClients");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvisorClients_Users_UsersUserID1",
                table: "AdvisorClients");

            migrationBuilder.DropIndex(
                name: "IX_AdvisorClients_UsersUserID",
                table: "AdvisorClients");

            migrationBuilder.DropIndex(
                name: "IX_AdvisorClients_UsersUserID1",
                table: "AdvisorClients");

            migrationBuilder.DropColumn(
                name: "UsersUserID",
                table: "AdvisorClients");

            migrationBuilder.DropColumn(
                name: "UsersUserID1",
                table: "AdvisorClients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersUserID",
                table: "AdvisorClients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersUserID1",
                table: "AdvisorClients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdvisorClients_UsersUserID",
                table: "AdvisorClients",
                column: "UsersUserID");

            migrationBuilder.CreateIndex(
                name: "IX_AdvisorClients_UsersUserID1",
                table: "AdvisorClients",
                column: "UsersUserID1");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvisorClients_Users_UsersUserID",
                table: "AdvisorClients",
                column: "UsersUserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvisorClients_Users_UsersUserID1",
                table: "AdvisorClients",
                column: "UsersUserID1",
                principalTable: "Users",
                principalColumn: "UserID");
        }
    }
}
