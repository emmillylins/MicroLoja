using MicroLoja.ProdutoAPI.Aplicacao.DTOs;

namespace MicroLoja.ProdutoAPI.Aplicacao.Interfaces
{
    public interface ICategoriaServico
    {
        Task<IEnumerable<CategoriaDto>> ObterTodasCategoriasAsync();
        Task<CategoriaDto?> ObterCategoriaPorIdAsync(int id);
        Task<CategoriaDto?> InserirCategoriaAsync(CriarAtualizarCategoriaDto categoriaDto);
        Task<bool> AtualizarCategoriaAsync(int id, CriarAtualizarCategoriaDto categoriaDto);
        Task<bool> ExcluirCategoriaAsync(int id);
    }
}
