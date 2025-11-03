using Microsoft.EntityFrameworkCore.Storage;

namespace MicroLoja.ProdutoAPI.Dominio.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<IDbContextTransaction> IniciarTransacaoAsync();
        Task CommitAsync();
        Task RollbackAsync();
        
        Task<bool> SalvarAlteracoesAsync();
        
        bool ExisteTransacaoAtiva { get; }
        IDbContextTransaction? TransacaoAtual { get; }
    }
}