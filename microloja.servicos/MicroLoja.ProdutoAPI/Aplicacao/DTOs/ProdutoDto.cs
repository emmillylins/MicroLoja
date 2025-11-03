namespace MicroLoja.ProdutoAPI.Aplicacao.DTOs
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int? CategoriaId { get; set; }
        public string? ImagemUrl { get; set; }
        public CategoriaDto? Categoria { get; set; }
    }

    public class CriarAtualizarProdutoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int? CategoriaId { get; set; }
        public string? ImagemUrl { get; set; }
    }
}