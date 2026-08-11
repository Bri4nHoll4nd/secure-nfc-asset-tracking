using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureNfc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUidAsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tags_Uid",
                table: "Tags",
                column: "Uid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_Uid",
                table: "Tags");
        }
    }
}
