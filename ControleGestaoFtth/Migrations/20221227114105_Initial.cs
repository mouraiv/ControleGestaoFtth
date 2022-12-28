using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleGestaoFtth.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.CreateTable(
                name: "estacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeEstacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sigla = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Responsavel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadoCampos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoCampos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "netwins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_netwins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "states",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstadodeProjeto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadodeControle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_states", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tipoObras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipoObras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Externo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "construtoras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CHAVE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstacoesId = table.Column<int>(type: "int", nullable: true),
                    TipoObraId = table.Column<int>(type: "int", nullable: true),
                    CDO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cabo = table.Column<int>(type: "int", nullable: true),
                    Celula = table.Column<int>(type: "int", nullable: true),
                    Capacidade = table.Column<int>(type: "int", nullable: true),
                    TotalUms = table.Column<int>(type: "int", nullable: true),
                    Endereco = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoCamposId = table.Column<int>(type: "int", nullable: true),
                    StatesId = table.Column<int>(type: "int", nullable: true),
                    AceitacaoData = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AceitacaoMesRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Viabilidade = table.Column<int>(type: "int", nullable: true),
                    Meta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatadeConstrucao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EquipedeConstrucao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatadoTeste = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Tecnico = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatadeRecebimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BobinaLancamento = table.Column<int>(type: "int", nullable: true),
                    BobinaRecepcao = table.Column<int>(type: "int", nullable: true),
                    QuantidadeDeTeste = table.Column<int>(type: "int", nullable: true),
                    PosicaoICXDGO = table.Column<string>(name: "PosicaoICX_DGO", type: "nvarchar(max)", nullable: true),
                    SplitterCEOS = table.Column<int>(type: "int", nullable: true),
                    FibraDGO = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_construtoras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_construtoras_EstadoCampos_EstadoCamposId",
                        column: x => x.EstadoCamposId,
                        principalTable: "EstadoCampos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_construtoras_estacoes_EstacoesId",
                        column: x => x.EstacoesId,
                        principalTable: "estacoes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_construtoras_states_StatesId",
                        column: x => x.StatesId,
                        principalTable: "states",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_construtoras_tipoObras_TipoObraId",
                        column: x => x.TipoObraId,
                        principalTable: "tipoObras",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tecnicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Funcao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tecnicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tecnicos_usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_construtoras_EstacoesId",
                table: "construtoras",
                column: "EstacoesId");

            migrationBuilder.CreateIndex(
                name: "IX_construtoras_EstadoCamposId",
                table: "construtoras",
                column: "EstadoCamposId");

            migrationBuilder.CreateIndex(
                name: "IX_construtoras_StatesId",
                table: "construtoras",
                column: "StatesId");

            migrationBuilder.CreateIndex(
                name: "IX_construtoras_TipoObraId",
                table: "construtoras",
                column: "TipoObraId");

            migrationBuilder.CreateIndex(
                name: "IX_tecnicos_UsuarioId",
                table: "tecnicos",
                column: "UsuarioId");*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "construtoras");

            migrationBuilder.DropTable(
                name: "netwins");

            migrationBuilder.DropTable(
                name: "tecnicos");

            migrationBuilder.DropTable(
                name: "EstadoCampos");

            migrationBuilder.DropTable(
                name: "estacoes");

            migrationBuilder.DropTable(
                name: "states");

            migrationBuilder.DropTable(
                name: "tipoObras");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
