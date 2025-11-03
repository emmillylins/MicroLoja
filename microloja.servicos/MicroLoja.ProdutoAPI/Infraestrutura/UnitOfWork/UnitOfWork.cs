using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MicroLoja.ProdutoAPI.Dominio.Interfaces;
using MicroLoja.ProdutoAPI.Dominio.Notificacoes;
using MicroLoja.ProdutoAPI.Infraestrutura.Contexto;

namespace MicroLoja.ProdutoAPI.Infraestrutura.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProdutoDbContext _contexto;
        private readonly INotificador _notificador;
        private IDbContextTransaction? _transacaoAtual;
        private bool _disposed = false;

        public UnitOfWork(ProdutoDbContext context, INotificador notificador)
        {
            _contexto = context;
            _notificador = notificador;
        }

        public bool ExisteTransacaoAtiva => _transacaoAtual != null;
        public IDbContextTransaction? TransacaoAtual => _transacaoAtual;

        public async Task<IDbContextTransaction> IniciarTransacaoAsync()
        {
            try
            {
                if (_transacaoAtual != null)
                {
                    _notificador.Handle(new Notificacao("Já existe uma transação ativa"));
                    return _transacaoAtual;
                }

                _transacaoAtual = await _contexto.Database.BeginTransactionAsync();
                return _transacaoAtual;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao iniciar transação: {ex.Message}"));
                throw;
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                if (_transacaoAtual == null)
                {
                    _notificador.Handle(new Notificacao("Não há transação ativa para confirmar"));
                    return;
                }

                await _transacaoAtual.CommitAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao confirmar transação: {ex.Message}"));
                await RollbackAsync();
                throw;
            }
            finally
            {
                if (_transacaoAtual != null)
                {
                    await _transacaoAtual.DisposeAsync();
                    _transacaoAtual = null;
                }
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                if (_transacaoAtual == null)
                {
                    _notificador.Handle(new Notificacao("Não há transação ativa para reverter"));
                    return;
                }

                await _transacaoAtual.RollbackAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao reverter transação: {ex.Message}"));
                throw;
            }
            finally
            {
                if (_transacaoAtual != null)
                {
                    await _transacaoAtual.DisposeAsync();
                    _transacaoAtual = null;
                }
            }
        }

        public async Task<bool> SalvarAlteracoesAsync()
        {
            try
            {
                var alteracoes = await _contexto.SaveChangesAsync();
                return alteracoes > 0;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao salvar alterações: {ex.Message}"));
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transacaoAtual?.Dispose();
                _disposed = true;
            }
        }
    }
}