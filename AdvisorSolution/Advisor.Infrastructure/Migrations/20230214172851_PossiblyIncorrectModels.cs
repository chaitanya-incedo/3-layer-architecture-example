using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Advisor.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PossiblyIncorrectModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvisorDetails",
                columns: table => new
                {
                    AdvisroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvisorDetails", x => x.AdvisroId);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentTypes",
                columns: table => new
                {
                    InvestmentTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestmentTypeName = table.Column<string>(type: "VARCHAR(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "DateTime2", nullable: true),
                    DeletedFlag = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentTypes", x => x.InvestmentTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "VARCHAR(15)", maxLength: 15, nullable: false),
                    Active = table.Column<byte>(type: "Tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true),
                    City = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: true),
                    State = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    Phone = table.Column<string>(type: "VARCHAR(40)", maxLength: 40, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(30)", maxLength: 30, nullable: false),
                    AdvisorID = table.Column<string>(type: "VARCHAR(6)", maxLength: 6, nullable: true),
                    AgentID = table.Column<string>(type: "VARCHAR(6)", maxLength: 6, nullable: true),
                    ClientID = table.Column<string>(type: "VARCHAR(6)", maxLength: 6, nullable: true),
                    LastName = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    Company = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: true),
                    SortName = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    Active = table.Column<byte>(type: "Tinyint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "DateTime2", nullable: true),
                    DeletedFlag = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    VerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestorInfos",
                columns: table => new
                {
                    InvestorInfoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    InvestmentName = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true),
                    Active = table.Column<byte>(type: "Tinyint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "DateTime2", nullable: true),
                    DeletedFlag = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestorInfos", x => x.InvestorInfoID);
                    table.ForeignKey(
                        name: "FK_InvestorInfos_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentStrategies",
                columns: table => new
                {
                    InvestmentStrategyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestorInfoID = table.Column<int>(type: "int", nullable: false),
                    StrategyName = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false),
                    AccountID = table.Column<string>(type: "VARCHAR(6)", maxLength: 6, nullable: false),
                    ModelAPLID = table.Column<string>(type: "VARCHAR(6)", maxLength: 6, nullable: false),
                    InvestmentAmount = table.Column<decimal>(type: "decimal(17,5)", nullable: false),
                    InvestmentTypeID = table.Column<int>(type: "int", nullable: false),
                    ModifiedBy = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "DateTime2", nullable: true),
                    DeletedFlag = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentStrategies", x => x.InvestmentStrategyID);
                    table.ForeignKey(
                        name: "FK_InvestmentStrategies_InvestmentTypes_InvestmentTypeID",
                        column: x => x.InvestmentTypeID,
                        principalTable: "InvestmentTypes",
                        principalColumn: "InvestmentTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentStrategies_InvestorInfos_InvestorInfoID",
                        column: x => x.InvestorInfoID,
                        principalTable: "InvestorInfos",
                        principalColumn: "InvestorInfoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentStrategies_InvestmentTypeID",
                table: "InvestmentStrategies",
                column: "InvestmentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentStrategies_InvestorInfoID",
                table: "InvestmentStrategies",
                column: "InvestorInfoID");

            migrationBuilder.CreateIndex(
                name: "IX_InvestorInfos_UserID",
                table: "InvestorInfos",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvisorDetails");

            migrationBuilder.DropTable(
                name: "InvestmentStrategies");

            migrationBuilder.DropTable(
                name: "InvestmentTypes");

            migrationBuilder.DropTable(
                name: "InvestorInfos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
