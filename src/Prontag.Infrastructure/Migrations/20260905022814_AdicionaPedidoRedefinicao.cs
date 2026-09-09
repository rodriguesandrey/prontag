using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prontag.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaPedidoRedefinicao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PedidoRedefinicao",
                table: "Usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PedidoRedefinicaoEm",
                table: "Usuarios",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PedidoRedefinicao",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "PedidoRedefinicaoEm",
                table: "Usuarios");
        }
    }
}
