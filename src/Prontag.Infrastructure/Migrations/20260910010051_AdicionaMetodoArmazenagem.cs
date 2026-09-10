using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prontag.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaMetodoArmazenagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetodoArmazenagem",
                table: "Produtos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetodoArmazenagem",
                table: "Produtos");
        }
    }
}
