using MicroLoja.ProdutoAPI.Aplicacao.DTOs;

namespace MicroLoja.ProdutoAPI.Aplicacao.Interfaces
{
    public interface IProdutoServico
    {
        // CRUD
        Task<ProdutoDto?> ObterPorIdAsync(int id);
        Task<IEnumerable<ProdutoDto>> ObterTodosAsync();
        Task<ProdutoDto?> InserirProdutoAsync(CriarAtualizarProdutoDto produto);
        Task<ProdutoDto?> AtualizarProdutoAsync(int id, CriarAtualizarProdutoDto produto);
        Task<bool> ExcluirProdutoAsync(int id);

        // Consultas Específicas
        Task<IEnumerable<ProdutoDto>> ObterProdutosPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<ProdutoDto>> BuscarPorNomeAsync(string nome);
        Task<IEnumerable<ProdutoDto>> ObterProdutosPorFaixaPrecoAsync(decimal precoMinimo, decimal precoMaximo);
        Task<IEnumerable<ProdutoDto>> ObterProdutosMaisCarosAsync(int quantidade = 10);
        Task<IEnumerable<ProdutoDto>> ObterProdutosMaisBaratosAsync(int quantidade = 10);

        // Validações e Verificações
        Task<bool> ExisteProdutoAsync(int id);
        Task<bool> ExisteProdutoComNomeAsync(string nome, int? excluirId = null);
        Task<int> ContarProdutosAsync();
        Task<int> ContarProdutosPorCategoriaAsync(int categoriaId);

        // Operações em Lote
        Task<IEnumerable<ProdutoDto>> InserirVariosProdutosAsync(IEnumerable<CriarAtualizarProdutoDto> produtos);
        Task<bool> ExcluirVariosProdutosAsync(IEnumerable<int> ids);
    }
}