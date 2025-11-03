using MicroLoja.ProdutoAPI.Dominio.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MicroLoja.ProdutoAPI.Infraestrutura.Seed
{
    public class ProdutoSeed
    {
        public static void Popular(ModelBuilder modelBuilder)
        {
            PopularCategorias(modelBuilder);
            PopularProdutos(modelBuilder);
        }

        private static void PopularCategorias(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, Nome = "Smartphones" },
                new Categoria { Id = 2, Nome = "Notebooks", Icone = "bi bi-laptop" },
                new Categoria { Id = 3, Nome = "Acessórios" },
                new Categoria { Id = 4, Nome = "TVs e Áudio" }
            );
        }

        private static void PopularProdutos(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>().HasData(
                new Produto
                {
                    Id = 1,
                    Nome = "Smartphone Galaxy S24",
                    Descricao = "Smartphone 5G com tela AMOLED de 6.6” e 256GB de armazenamento.",
                    ImagemUrl = "imagens/produtos/galaxy-s24.jpg",
                    Preco = 4299.90m,
                    CategoriaId = 1
                },
                new Produto
                {
                    Id = 2,
                    Nome = "Notebook Dell Inspiron 15",
                    Descricao = "Notebook com Intel Core i7, 16GB RAM e SSD de 512GB.",
                    ImagemUrl = "imagens/produtos/dell-inspiron-15.jpg",
                    Preco = 5899.00m,
                    CategoriaId = 2
                },
                new Produto
                {
                    Id = 3,
                    Nome = "Mouse Gamer RGB",
                    Descricao = "Mouse óptico com iluminação RGB e 6 botões programáveis.",
                    ImagemUrl = "imagens/produtos/mouse-gamer-rgb.jpg",
                    Preco = 199.90m,
                    CategoriaId = 3
                },
                new Produto
                {
                    Id = 4,
                    Nome = "Fone Bluetooth ANC Pro",
                    Descricao = "Fone de ouvido sem fio com cancelamento ativo de ruído.",
                    ImagemUrl = "imagens/produtos/fone-bluetooth-anc-pro.jpg",
                    Preco = 799.90m,
                    CategoriaId = 3
                },
                new Produto
                {
                    Id = 5,
                    Nome = "Smart TV 55” 4K UHD",
                    Descricao = "Smart TV LED 55 polegadas com suporte a HDR10+ e Dolby Audio.",
                    ImagemUrl = "imagens/produtos/smart-tv-55.jpg",
                    Preco = 3499.00m,
                    CategoriaId = 4
                },
                new Produto
                {
                    Id = 6,
                    Nome = "Teclado Mecânico RGB",
                    Descricao = "Teclado mecânico ABNT2 com switches vermelhos e retroiluminação RGB.",
                    ImagemUrl = "imagens/produtos/teclado-mecanico-rgb.jpg",
                    Preco = 459.90m,
                    CategoriaId = 3
                },
                new Produto
                {
                    Id = 7,
                    Nome = "Caixa de Som Bluetooth X300",
                    Descricao = "Caixa de som portátil com 24h de bateria e som estéreo potente.",
                    ImagemUrl = "imagens/produtos/caixa-som-x300.jpg",
                    Preco = 599.90m,
                    CategoriaId = 4
                }
            );
        }
    }
}
