using Microsoft.EntityFrameworkCore;
using MicroLoja.ProdutoAPI.Dominio.Interfaces;
using MicroLoja.ProdutoAPI.Dominio.Modelos;
using MicroLoja.ProdutoAPI.Infraestrutura.Contexto;
using MicroLoja.ProdutoAPI.Infraestrutura.Repositorios.Base;
using MicroLoja.ProdutoAPI.Dominio.Notificacoes;

namespace MicroLoja.ProdutoAPI.Infraestrutura.Repositorios
{
    public class ProdutoRepositorio : RepositorioBase<Produto>, IProdutoRepositorio
    {
        public ProdutoRepositorio(ProdutoDbContext context, INotificador notificador) 
            : base(context, notificador)
        {
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorCategoriaAsync(int categoriaId)
        {
            try
            {
                return await _dbSet
                    .Where(p => p.CategoriaId == categoriaId)
                    .Include(p => p.Categoria)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao buscar produtos da categoria {categoriaId}: {ex.Message}"));
                return Enumerable.Empty<Produto>();
            }
        }

        public async Task<IEnumerable<Produto>> ObterProdutosComCategoriaAsync()
        {
            try
            {
                return await _dbSet
                    .Include(p => p.Categoria)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao buscar produtos com categoria: {ex.Message}"));
                return Enumerable.Empty<Produto>();
            }
        }

        public async Task<Produto?> ObterProdutoComCategoriaAsync(int id)
        {
            try
            {
                return await _dbSet
                    .Include(p => p.Categoria)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao buscar produto com ID {id}: {ex.Message}"));
                return null;
            }
        }

        public async Task<IEnumerable<Produto>> BuscarPorNomeAsync(string nome)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                {
                    _notificador.Handle(new Notificacao("Nome não pode ser nulo ou vazio"));
                    return Enumerable.Empty<Produto>();
                }

                return await _dbSet
                    .Where(p => p.Nome.Contains(nome))
                    .Include(p => p.Categoria)
                    .OrderBy(p => p.Nome)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao buscar produtos por nome '{nome}': {ex.Message}"));
                return Enumerable.Empty<Produto>();
            }
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFaixaPrecoAsync(decimal precoMinimo, decimal precoMaximo)
        {
            try
            {
                if (precoMinimo < 0)
                {
                    _notificador.Handle(new Notificacao("Preço mínimo não pode ser negativo"));
                    return Enumerable.Empty<Produto>();
                }

                if (precoMaximo < 0)
                {
                    _notificador.Handle(new Notificacao("Preço máximo não pode ser negativo"));
                    return Enumerable.Empty<Produto>();
                }

                if (precoMinimo > precoMaximo)
                {
                    _notificador.Handle(new Notificacao("Preço mínimo não pode ser maior que o preço máximo"));
                    return Enumerable.Empty<Produto>();
                }

                return await _dbSet
                    .Where(p => p.Preco >= precoMinimo && p.Preco <= precoMaximo)
                    .Include(p => p.Categoria)
                    .OrderBy(p => p.Preco)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao buscar produtos por faixa de preço: {ex.Message}"));
                return Enumerable.Empty<Produto>();
            }
        }

        public async Task<IEnumerable<Produto>> ObterProdutosMaisCarosAsync(int quantidade = 10)
        {
            try
            {
                if (quantidade <= 0)
                {
                    _notificador.Handle(new Notificacao("Quantidade deve ser maior que zero"));
                    return Enumerable.Empty<Produto>();
                }

                return await _dbSet
                    .Include(p => p.Categoria)
                    .OrderByDescending(p => p.Preco)
                    .Take(quantidade)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao buscar produtos mais caros: {ex.Message}"));
                return Enumerable.Empty<Produto>();
            }
        }

        public async Task<IEnumerable<Produto>> ObterProdutosMaisBaratosAsync(int quantidade = 10)
        {
            try
            {
                if (quantidade <= 0)
                {
                    _notificador.Handle(new Notificacao("Quantidade deve ser maior que zero"));
                    return Enumerable.Empty<Produto>();
                }

                return await _dbSet
                    .Include(p => p.Categoria)
                    .OrderBy(p => p.Preco)
                    .Take(quantidade)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao buscar produtos mais baratos: {ex.Message}"));
                return Enumerable.Empty<Produto>();
            }
        }

        public async Task<bool> ExisteProdutoComNomeAsync(string nome, int? excluirId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                {
                    _notificador.Handle(new Notificacao("Nome não pode ser nulo ou vazio"));
                    return false;
                }

                var query = _dbSet.Where(p => p.Nome.ToLower() == nome.ToLower());
                
                if (excluirId.HasValue)
                {
                    query = query.Where(p => p.Id != excluirId.Value);
                }

                return await query.AnyAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao verificar existência do produto '{nome}': {ex.Message}"));
                return false;
            }
        }

        public override async Task<Produto?> ObterPorIdAsync(int id)
        {
            try
            {
                return await _dbSet
                    .Include(p => p.Categoria)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao buscar produto com ID {id}: {ex.Message}"));
                return null;
            }
        }

        public override async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            try
            {
                return await _dbSet
                    .Include(p => p.Categoria)
                    .OrderBy(p => p.Nome)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao buscar todos os produtos: {ex.Message}"));
                return Enumerable.Empty<Produto>();
            }
        }
    }
}