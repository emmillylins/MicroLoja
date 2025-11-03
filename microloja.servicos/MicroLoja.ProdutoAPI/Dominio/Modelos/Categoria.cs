using MicroLoja.ProdutoAPI.Dominio.Modelos.Base;

namespace MicroLoja.ProdutoAPI.Dominio.Modelos
{
    public class Categoria : BaseEntity
    {
        public Categoria() { }
        public Categoria(string nome, string? icone)
        {
            Nome = nome;
            Icone = icone;
        }

        public string Nome { get; set; }
        public string? Icone { get; set; }


        public List<Produto> Produtos { get; set; }
    }
}
