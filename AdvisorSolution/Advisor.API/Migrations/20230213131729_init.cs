using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Advisor.API.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "AdvisorDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "AdvisorDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerfiicationTokenForReset",
                table: "AdvisorDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "AdvisorDetails",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "AdvisorDetails");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "AdvisorDetails");

            migrationBuilder.DropColumn(
                name: "VerfiicationTokenForReset",
                table: "AdvisorDetails");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "AdvisorDetails");
        }
    }
}
