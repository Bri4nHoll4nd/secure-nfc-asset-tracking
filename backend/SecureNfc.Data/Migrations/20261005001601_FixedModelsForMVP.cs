using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureNfc.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedModelsForMVP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assets_EntityCode",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "EntityCode",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "EntityCode",
                table: "Assets",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Assets",
                newName: "EntityCode");

            migrationBuilder.AddColumn<string>(
                name: "EntityCode",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_EntityCode",
                table: "Assets",
                column: "EntityCode",
                unique: true);
        }
    }
}
