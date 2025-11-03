using MicroLoja.ProdutoAPI.Dominio.Modelos.Base;

namespace MicroLoja.ProdutoAPI.Dominio.Modelos
{
    public class Produto : BaseEntity
    {
        public Produto() { }

        public Produto(string nome, string? descricao, decimal preco, int? categoriaId, string? imagemUrl)
        {
            Nome = nome;
            Descricao = descricao;
            Preco = preco;
            CategoriaId = categoriaId;
            ImagemUrl = imagemUrl;
        }

        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int? CategoriaId { get; set; }
        public string? ImagemUrl { get; set; }

        public Categoria Categoria { get; set; }
    }
}
