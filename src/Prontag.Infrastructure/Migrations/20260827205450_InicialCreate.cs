using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prontag.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InicialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Funcionarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcionarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    DiasValidade = table.Column<int>(type: "INTEGER", nullable: false),
                    Preco = table.Column<decimal>(type: "TEXT", nullable: true),
                    Icone = table.Column<string>(type: "TEXT", nullable: true),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelatoriosMensais",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MesAno = table.Column<string>(type: "TEXT", nullable: false),
                    TotalImpressoes = table.Column<int>(type: "INTEGER", nullable: false),
                    FuncionarioTopId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ProdutoTopId = table.Column<Guid>(type: "TEXT", nullable: true),
                    BreakdownJson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatoriosMensais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Impressoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TenantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Fabricacao = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Vencimento = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impressoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Impressoes_Funcionarios_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Impressoes_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Impressoes_FuncionarioId",
                table: "Impressoes",
                column: "FuncionarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Impressoes_ProdutoId",
                table: "Impressoes",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Impressoes");

            migrationBuilder.DropTable(
                name: "RelatoriosMensais");

            migrationBuilder.DropTable(
                name: "Funcionarios");

            migrationBuilder.DropTable(
                name: "Produtos");
        }
    }
}
