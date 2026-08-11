using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Postgres.Api.Migrations
{
    /// <inheritdoc />
    public partial class MakeECUniqueRetry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tags_EntityCode",
                table: "Tags",
                column: "EntityCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tags_EntityCode",
                table: "Tags");
        }
    }
}
