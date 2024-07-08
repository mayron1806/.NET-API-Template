using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_files_entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Plan = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PlanActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrganizationId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsOwner = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => new { x.UserId, x.OrganizationId });
                    table.ForeignKey(
                        name: "FK_Member_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Member_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transfer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Key = table.Column<string>(type: "TEXT", maxLength: 24, nullable: false),
                    OrganizationId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "NOW()"),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    FilesCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    TransferType = table.Column<int>(type: "INTEGER", nullable: false),
                    Receive_Received = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false),
                    Receive_Message = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Receive_AcceptedFiles = table.Column<string>(type: "jsonb", nullable: true),
                    Receive_MaxSize = table.Column<int>(type: "INTEGER", nullable: true),
                    Receive_Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Send_Message = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Send_Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Send_Downloads = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    Send_QuickDownload = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false),
                    Send_ExpiresOnDowload = table.Column<bool>(type: "INTEGER", nullable: true, defaultValue: false),
                    Send_Destination = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfer_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OriginalName = table.Column<string>(type: "TEXT", nullable: false),
                    Path = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", nullable: false),
                    TransferId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                    table.ForeignKey(
                        name: "FK_File_Transfer_TransferId",
                        column: x => x.TransferId,
                        principalTable: "Transfer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_File_TransferId",
                table: "File",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_OrganizationId",
                table: "Member",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_Key",
                table: "Transfer",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transfer_OrganizationId",
                table: "Transfer",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "Transfer");

            migrationBuilder.DropTable(
                name: "Organization");
        }
    }
}
