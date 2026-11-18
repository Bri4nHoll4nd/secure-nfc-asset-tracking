using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureNfc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetRegistrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssetRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReaderId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    MaintenanceStatus = table.Column<string>(type: "text", nullable: false),
                    Uid = table.Column<string>(type: "text", nullable: true),
                    Version = table.Column<string>(type: "text", nullable: false),
                    SignatureHex = table.Column<string>(type: "text", nullable: true),
                    RegistrationStatus = table.Column<string>(type: "text", nullable: false),
                    Error = table.Column<string>(type: "text", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetRegistrations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetRegistrations_ReaderId",
                table: "AssetRegistrations",
                column: "ReaderId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRegistrations_Uid",
                table: "AssetRegistrations",
                column: "Uid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetRegistrations");
        }
    }
}
