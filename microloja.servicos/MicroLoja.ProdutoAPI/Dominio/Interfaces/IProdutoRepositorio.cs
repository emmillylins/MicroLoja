using MicroLoja.ProdutoAPI.Dominio.Interfaces.Base;
using MicroLoja.ProdutoAPI.Dominio.Modelos;

namespace MicroLoja.ProdutoAPI.Dominio.Interfaces
{
    public interface IProdutoRepositorio : IRepositorioBase<Produto>
    {
        Task<IEnumerable<Produto>> ObterProdutosPorCategoriaAsync(int categoriaId);
        Task<IEnumerable<Produto>> ObterProdutosComCategoriaAsync();
        Task<Produto?> ObterProdutoComCategoriaAsync(int id);
        Task<IEnumerable<Produto>> BuscarPorNomeAsync(string nome);
        Task<IEnumerable<Produto>> ObterProdutosPorFaixaPrecoAsync(decimal precoMinimo, decimal precoMaximo);
        Task<IEnumerable<Produto>> ObterProdutosMaisCarosAsync(int quantidade = 10);
        Task<IEnumerable<Produto>> ObterProdutosMaisBaratosAsync(int quantidade = 10);
        Task<bool> ExisteProdutoComNomeAsync(string nome, int? excluirId = null);
    }
}