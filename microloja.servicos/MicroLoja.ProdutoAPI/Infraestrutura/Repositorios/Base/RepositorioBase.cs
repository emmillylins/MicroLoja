using System.Data.SqlClient;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MicroLoja.ProdutoAPI.Infraestrutura.Contexto;
using MicroLoja.ProdutoAPI.Dominio.Interfaces.Base;
using MicroLoja.ProdutoAPI.Dominio.Notificacoes;
using Microsoft.Data.SqlClient;

namespace MicroLoja.ProdutoAPI.Infraestrutura.Repositorios.Base
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : class
    {
        protected readonly ProdutoDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly INotificador _notificador;

        public RepositorioBase(ProdutoDbContext context, INotificador notificador)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _notificador = notificador;
        }

        public virtual async Task<T?> ObterPorIdAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao buscar {typeof(T).Name} com ID {id}: {ex.Message}"));
                return null;
            }
        }

        public virtual async Task<IEnumerable<T>> ObterTodosAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao buscar todas as entidades {typeof(T).Name}: {ex.Message}"));
                return Enumerable.Empty<T>();
            }
        }

        public virtual async Task<IEnumerable<T>> BuscarAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                if (predicate == null)
                {
                    _notificador.Handle(new Notificacao("Filtro de busca não pode ser nulo"));
                    return Enumerable.Empty<T>();
                }

                return await _dbSet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao buscar {typeof(T).Name} com filtro: {ex.Message}"));
                return Enumerable.Empty<T>();
            }
        }

        public virtual async Task<T?> PrimeiroOuPadraoAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                if (predicate == null)
                {
                    _notificador.Handle(new Notificacao("Filtro de busca não pode ser nulo"));
                    return null;
                }

                return await _dbSet.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao buscar primeira {typeof(T).Name}: {ex.Message}"));
                return null;
            }
        }

        public virtual async Task<T?> InserirAsync(T entidade)
        {
            try
            {
                if (entidade == null)
                {
                    _notificador.Handle(new Notificacao("Entidade não pode ser nula"));
                    return null;
                }

                await _dbSet.AddAsync(entidade);
                return entidade;
            }
            catch (SqlException sqlEx)
            {
                var innerEx = sqlEx.InnerException ?? sqlEx;
                _notificador.Handle(new Notificacao($"Erro de banco de dados ao Inserir {typeof(T).Name}: {innerEx.Message}"));
                return null;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao adicionar {typeof(T).Name}: {ex.Message}"));
                return null;
            }
        }

        public virtual async Task<IEnumerable<T>?> InserirVariosAsync(IEnumerable<T> entidades)
        {
            try
            {
                if (entidades == null)
                {
                    _notificador.Handle(new Notificacao("Lista de entidades não pode ser nula"));
                    return null;
                }

                var lista = entidades.ToList();
                if (!lista.Any())
                {
                    _notificador.Handle(new Notificacao("A lista de entidades não pode estar vazia"));
                    return null;
                }

                await _dbSet.AddRangeAsync(lista);
                return lista;
            }
            catch (SqlException sqlEx)
            {
                var innerEx = sqlEx.InnerException ?? sqlEx;
                _notificador.Handle(new Notificacao($"Erro de banco de dados ao adicionar múltiplas {typeof(T).Name}: {innerEx.Message}"));
                return null;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao adicionar múltiplas {typeof(T).Name}: {ex.Message}"));
                return null;
            }
        }

        public virtual async Task<bool> AtualizarAsync(T entidade)
        {
            try
            {
                if (entidade == null)
                {
                    _notificador.Handle(new Notificacao("Entidade não pode ser nula"));
                    return false;
                }

                _dbSet.Update(entidade);
                await Task.CompletedTask;
                return true;
            }
            catch (SqlException sqlEx)
            {
                var innerEx = sqlEx.InnerException ?? sqlEx;
                _notificador.Handle(new Notificacao($"Erro de banco de dados ao atualizar {typeof(T).Name}: {innerEx.Message}"));
                return false;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao atualizar {typeof(T).Name}: {ex.Message}"));
                return false;
            }
        }

        public virtual async Task<bool> ExcluirAsync(T entidade)
        {
            try
            {
                if (entidade == null)
                {
                    _notificador.Handle(new Notificacao("Entidade não pode ser nula"));
                    return false;
                }

                _dbSet.Remove(entidade);
                await Task.CompletedTask;
                return true;
            }
            catch (SqlException sqlEx)
            {
                var innerEx = sqlEx.InnerException ?? sqlEx;
                _notificador.Handle(new Notificacao($"Erro de banco de dados ao remover {typeof(T).Name}: {innerEx.Message}"));
                return false;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao remover {typeof(T).Name}: {ex.Message}"));
                return false;
            }
        }

        public virtual async Task<bool> ExcluirPorIdAsync(int id)
        {
            try
            {
                var entidade = await ObterPorIdAsync(id);
                if (entidade != null)
                {
                    return await ExcluirAsync(entidade);
                }
                
                _notificador.Handle(new Notificacao($"{typeof(T).Name} com ID {id} não encontrado para remoção"));
                return false;
            }
            catch (SqlException sqlEx)
            {
                var innerEx = sqlEx.InnerException ?? sqlEx;
                _notificador.Handle(new Notificacao($"Erro de banco de dados ao remover {typeof(T).Name} com ID {id}: {innerEx.Message}"));
                return false;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao remover {typeof(T).Name} com ID {id}: {ex.Message}"));
                return false;
            }
        }

        public virtual async Task<bool> ExisteAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id) != null;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao verificar existência de {typeof(T).Name} com ID {id}: {ex.Message}"));
                return false;
            }
        }

        public virtual async Task<bool> ExisteAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                if (predicate == null)
                {
                    _notificador.Handle(new Notificacao("Filtro de busca não pode ser nulo"));
                    return false;
                }

                return await _dbSet.AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao verificar existência de {typeof(T).Name}: {ex.Message}"));
                return false;
            }
        }

        public virtual async Task<int> ContarAsync()
        {
            try
            {
                return await _dbSet.CountAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao contar {typeof(T).Name}: {ex.Message}"));
                return 0;
            }
        }

        public virtual async Task<int> ContarAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                if (predicate == null)
                {
                    _notificador.Handle(new Notificacao("Filtro de busca não pode ser nulo"));
                    return 0;
                }

                return await _dbSet.CountAsync(predicate);
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao contar {typeof(T).Name} com filtro: {ex.Message}"));
                return 0;
            }
        }

        public virtual async Task<bool> SalvarAlteracoesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _notificador.Handle(new Notificacao($"Erro de concorrência: Os dados foram modificados por outro usuário. {ex.Message}"));
                return false;
            }
            catch (DbUpdateException ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao salvar no banco de dados: {ex.Message}"));
                return false;
            }
            catch (SqlException sqlEx)
            {
                var innerEx = sqlEx.InnerException ?? sqlEx;
                _notificador.Handle(new Notificacao($"Erro de banco de dados aoSalvar alterações: {innerEx.Message}"));
                return false;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro inesperado ao salvar alterações: {ex.Message}"));
                return false;
            }
        }
    }
}