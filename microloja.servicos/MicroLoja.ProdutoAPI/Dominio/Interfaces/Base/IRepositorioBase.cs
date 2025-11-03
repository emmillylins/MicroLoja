using System.Linq.Expressions;

namespace MicroLoja.ProdutoAPI.Dominio.Interfaces.Base
{
    public interface IRepositorioBase<T> where T : class
    {
        Task<T?> ObterPorIdAsync(int id);
        Task<IEnumerable<T>> ObterTodosAsync();
        Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicate);
        Task<T?> PrimeiroOuPadraoAsync(Expression<Func<T, bool>> predicate);
        Task<T?> InserirAsync(T entidade);
        Task<IEnumerable<T>?> InserirVariosAsync(IEnumerable<T> entidades);
        Task<bool> AtualizarAsync(T entidade);
        Task<bool> ExcluirAsync(T entidade);
        Task<bool> ExcluirPorIdAsync(int id);
        Task<bool> ExisteAsync(int id);
        Task<bool> ExisteAsync(Expression<Func<T, bool>> predicate);
        Task<int> ContarAsync();
        Task<int> ContarAsync(Expression<Func<T, bool>> predicate);
        Task<bool> SalvarAlteracoesAsync();
    }
}