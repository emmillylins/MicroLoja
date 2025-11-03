namespace MicroLoja.ProdutoAPI.Aplicacao.DTOs
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Icone { get; set; }
    }
    public class CriarAtualizarCategoriaDto
    {
        public string Nome { get; set; } = string.Empty;
        public string? Icone { get; set; }
    }
}