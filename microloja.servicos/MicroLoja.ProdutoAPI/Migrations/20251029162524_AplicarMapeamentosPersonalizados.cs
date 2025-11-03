using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MicroLoja.ProdutoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AplicarMapeamentosPersonalizados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "produtos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    preco = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    categoria_id = table.Column<int>(type: "int", nullable: true),
                    imagem_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produtos", x => x.id);
                    table.ForeignKey(
                        name: "FK_produtos_categorias_categoria_id",
                        column: x => x.categoria_id,
                        principalTable: "categorias",
                        principalColumn: "id");
                });

            migrationBuilder.InsertData(
                table: "categorias",
                columns: new[] { "id", "nome" },
                values: new object[,]
                {
                    { 1, "Smartphones" },
                    { 2, "Notebooks" },
                    { 3, "Acessórios" },
                    { 4, "TVs e Áudio" }
                });

            migrationBuilder.InsertData(
                table: "produtos",
                columns: new[] { "id", "categoria_id", "descricao", "imagem_url", "nome", "preco" },
                values: new object[,]
                {
                    { 1, 1, "Smartphone 5G com tela AMOLED de 6.6” e 256GB de armazenamento.", "imagens/produtos/galaxy-s24.jpg", "Smartphone Galaxy S24", 4299.90m },
                    { 2, 2, "Notebook com Intel Core i7, 16GB RAM e SSD de 512GB.", "imagens/produtos/dell-inspiron-15.jpg", "Notebook Dell Inspiron 15", 5899.00m },
                    { 3, 3, "Mouse óptico com iluminação RGB e 6 botões programáveis.", "imagens/produtos/mouse-gamer-rgb.jpg", "Mouse Gamer RGB", 199.90m },
                    { 4, 3, "Fone de ouvido sem fio com cancelamento ativo de ruído.", "imagens/produtos/fone-bluetooth-anc-pro.jpg", "Fone Bluetooth ANC Pro", 799.90m },
                    { 5, 4, "Smart TV LED 55 polegadas com suporte a HDR10+ e Dolby Audio.", "imagens/produtos/smart-tv-55.jpg", "Smart TV 55” 4K UHD", 3499.00m },
                    { 6, 3, "Teclado mecânico ABNT2 com switches vermelhos e retroiluminação RGB.", "imagens/produtos/teclado-mecanico-rgb.jpg", "Teclado Mecânico RGB", 459.90m },
                    { 7, 4, "Caixa de som portátil com 24h de bateria e som estéreo potente.", "imagens/produtos/caixa-som-x300.jpg", "Caixa de Som Bluetooth X300", 599.90m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_produtos_categoria_id",
                table: "produtos",
                column: "categoria_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "produtos");

            migrationBuilder.DropTable(
                name: "categorias");
        }
    }
}
