using AutoMapper;
using FluentValidation;
using MicroLoja.ProdutoAPI.Aplicacao.DTOs;
using MicroLoja.ProdutoAPI.Aplicacao.Interfaces;
using MicroLoja.ProdutoAPI.Dominio.Interfaces;
using MicroLoja.ProdutoAPI.Dominio.Modelos;
using MicroLoja.ProdutoAPI.Dominio.Notificacoes;

namespace MicroLoja.ProdutoAPI.Aplicacao.Servicos
{
    public class ProdutoServico : IProdutoServico
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly INotificador _notificador;
        private readonly IValidator<Produto> _produtoValidator;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProdutoServico(IProdutoRepositorio produtoRepositorio, 
                             INotificador notificador,
                             IValidator<Produto> produtoValidator,
                             IMapper mapper,
                             IUnitOfWork unitOfWork)
        {
            _produtoRepositorio = produtoRepositorio;
            _notificador = notificador;
            _produtoValidator = produtoValidator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #region CRUD
        public async Task<ProdutoDto?> ObterPorIdAsync(int id)
        {
            if (id <= 0)
            {
                _notificador.Handle(new Notificacao("ID deve ser maior que zero"));
                return null;
            }

            var produto = await _produtoRepositorio.ObterProdutoComCategoriaAsync(id);
            return produto != null ? _mapper.Map<ProdutoDto>(produto) : null;
        }

        public async Task<IEnumerable<ProdutoDto>> ObterTodosAsync()
        {
            var produtos = await _produtoRepositorio.ObterProdutosComCategoriaAsync();
            return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        }

        public async Task<ProdutoDto?> InserirProdutoAsync(CriarAtualizarProdutoDto produtoDto)
        {
            if (produtoDto == null)
            {
                _notificador.Handle(new Notificacao("Produto não pode ser nulo"));
                return null;
            }

            await _unitOfWork.IniciarTransacaoAsync();
            
            try
            {
                var produto = _mapper.Map<Produto>(produtoDto);

                if (!await ValidarProdutoAsync(produto))
                    return null;

                if (await _produtoRepositorio.ExisteProdutoComNomeAsync(produto.Nome))
                {
                    _notificador.Handle(new Notificacao($"Já existe um produto com o nome '{produto.Nome}'"));
                    return null;
                }

                var produtoCriado = await _produtoRepositorio.InserirAsync(produto);
                if (produtoCriado == null)
                    return null;

                if (!await _unitOfWork.SalvarAlteracoesAsync())
                    return null;

                await _unitOfWork.CommitAsync();

                var produtoComCategoria = await _produtoRepositorio.ObterProdutoComCategoriaAsync(produtoCriado.Id);
                return produtoComCategoria != null ? _mapper.Map<ProdutoDto>(produtoComCategoria) : null;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao inserir produto: {ex.Message}"));
                throw;
            }
            finally
            {
                if (_unitOfWork.ExisteTransacaoAtiva)
                {
                    await _unitOfWork.RollbackAsync();
                }
            }
        }

        public async Task<ProdutoDto?> AtualizarProdutoAsync(int id, CriarAtualizarProdutoDto produtoDto)
        {
            if (id <= 0)
            {
                _notificador.Handle(new Notificacao("ID deve ser maior que zero"));
                return null;
            }

            if (produtoDto == null)
            {
                _notificador.Handle(new Notificacao("Produto não pode ser nulo"));
                return null;
            }

            await _unitOfWork.IniciarTransacaoAsync();

            try
            {
                var produtoExistente = await _produtoRepositorio.ObterPorIdAsync(id);
                if (produtoExistente == null)
                {
                    _notificador.Handle(new Notificacao("Produto não encontrado"));
                    return null;
                }

                _mapper.Map(produtoDto, produtoExistente);

                if (!await ValidarProdutoAsync(produtoExistente))
                    return null;

                if (await _produtoRepositorio.ExisteProdutoComNomeAsync(produtoExistente.Nome, id))
                {
                    _notificador.Handle(new Notificacao($"Já existe outro produto com o nome '{produtoExistente.Nome}'"));
                    return null;
                }

                if (!await _produtoRepositorio.AtualizarAsync(produtoExistente))
                    return null;

                if (!await _unitOfWork.SalvarAlteracoesAsync())
                    return null;

                await _unitOfWork.CommitAsync();

                var produtoAtualizado = await _produtoRepositorio.ObterProdutoComCategoriaAsync(id);
                return produtoAtualizado != null ? _mapper.Map<ProdutoDto>(produtoAtualizado) : null;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao atualizar produto: {ex.Message}"));
                throw;
            }
            finally
            {
                if (_unitOfWork.ExisteTransacaoAtiva)
                {
                    await _unitOfWork.RollbackAsync();
                }
            }
        }

        public async Task<bool> ExcluirProdutoAsync(int id)
        {
            if (id <= 0)
            {
                _notificador.Handle(new Notificacao("ID deve ser maior que zero"));
                return false;
            }

            await _unitOfWork.IniciarTransacaoAsync();

            try
            {
                var produto = await _produtoRepositorio.ObterPorIdAsync(id);
                if (produto == null)
                {
                    _notificador.Handle(new Notificacao($"Produto com ID {id} não encontrado"));
                    return false;
                }

                if (!await _produtoRepositorio.ExcluirAsync(produto))
                    return false;

                if (!await _unitOfWork.SalvarAlteracoesAsync())
                    return false;

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao excluir produto: {ex.Message}"));
                return false;
            }
            finally
            {
                if (_unitOfWork.ExisteTransacaoAtiva)
                {
                    await _unitOfWork.RollbackAsync();
                }
            }
        }
        #endregion

        #region Consultas Específicas
        public async Task<IEnumerable<ProdutoDto>> ObterProdutosPorCategoriaAsync(int categoriaId)
        {
            if (categoriaId <= 0)
            {
                _notificador.Handle(new Notificacao("ID da categoria deve ser maior que zero"));
                return Enumerable.Empty<ProdutoDto>();
            }

            var produtos = await _produtoRepositorio.ObterProdutosPorCategoriaAsync(categoriaId);
            return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        }

        public async Task<IEnumerable<ProdutoDto>> BuscarPorNomeAsync(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                _notificador.Handle(new Notificacao("Nome não pode ser nulo ou vazio"));
                return Enumerable.Empty<ProdutoDto>();
            }

            var produtos = await _produtoRepositorio.BuscarPorNomeAsync(nome);
            return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        }

        public async Task<IEnumerable<ProdutoDto>> ObterProdutosPorFaixaPrecoAsync(decimal precoMinimo, decimal precoMaximo)
        {
            if (precoMinimo < 0)
            {
                _notificador.Handle(new Notificacao("Preço mínimo não pode ser negativo"));
                return Enumerable.Empty<ProdutoDto>();
            }

            if (precoMaximo < 0)
            {
                _notificador.Handle(new Notificacao("Preço máximo não pode ser negativo"));
                return Enumerable.Empty<ProdutoDto>();
            }

            if (precoMinimo > precoMaximo)
            {
                _notificador.Handle(new Notificacao("Preço mínimo não pode ser maior que o preço máximo"));
                return Enumerable.Empty<ProdutoDto>();
            }

            var produtos = await _produtoRepositorio.ObterProdutosPorFaixaPrecoAsync(precoMinimo, precoMaximo);
            return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        }

        public async Task<IEnumerable<ProdutoDto>> ObterProdutosMaisCarosAsync(int quantidade = 10)
        {
            if (quantidade <= 0)
            {
                _notificador.Handle(new Notificacao("Quantidade deve ser maior que zero"));
                return Enumerable.Empty<ProdutoDto>();
            }

            var produtos = await _produtoRepositorio.ObterProdutosMaisCarosAsync(quantidade);
            return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        }

        public async Task<IEnumerable<ProdutoDto>> ObterProdutosMaisBaratosAsync(int quantidade = 10)
        {
            if (quantidade <= 0)
            {
                _notificador.Handle(new Notificacao("Quantidade deve ser maior que zero"));
                return Enumerable.Empty<ProdutoDto>();
            }

            var produtos = await _produtoRepositorio.ObterProdutosMaisBaratosAsync(quantidade);
            return _mapper.Map<IEnumerable<ProdutoDto>>(produtos);
        }
        #endregion

        #region Validações e Verificações
        public async Task<bool> ExisteProdutoAsync(int id)
        {
            if (id <= 0)
            {
                _notificador.Handle(new Notificacao("ID deve ser maior que zero"));
                return false;
            }

            return await _produtoRepositorio.ExisteAsync(id);
        }

        public async Task<bool> ExisteProdutoComNomeAsync(string nome, int? excluirId = null)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                _notificador.Handle(new Notificacao("Nome não pode ser nulo ou vazio"));
                return false;
            }

            return await _produtoRepositorio.ExisteProdutoComNomeAsync(nome, excluirId);
        }

        public async Task<int> ContarProdutosAsync()
        {
            return await _produtoRepositorio.ContarAsync();
        }

        public async Task<int> ContarProdutosPorCategoriaAsync(int categoriaId)
        {
            if (categoriaId <= 0)
            {
                _notificador.Handle(new Notificacao("ID da categoria deve ser maior que zero"));
                return 0;
            }

            return await _produtoRepositorio.ContarAsync(p => p.CategoriaId == categoriaId);
        }
        #endregion

        #region Operações em Lote
        public async Task<IEnumerable<ProdutoDto>> InserirVariosProdutosAsync(IEnumerable<CriarAtualizarProdutoDto> produtosDto)
        {
            if (produtosDto == null)
            {
                _notificador.Handle(new Notificacao("Lista de produtos não pode ser nula"));
                return Enumerable.Empty<ProdutoDto>();
            }

            var listaProdutosDto = produtosDto.ToList();
            if (!listaProdutosDto.Any())
            {
                _notificador.Handle(new Notificacao("Lista de produtos não pode estar vazia"));
                return Enumerable.Empty<ProdutoDto>();
            }

            var transacao = await _unitOfWork.IniciarTransacaoAsync();
            var produtosCriados = new List<ProdutoDto>();

            try
            {
                foreach (var produtoDto in listaProdutosDto)
                {
                    var produto = _mapper.Map<Produto>(produtoDto);

                    if (!await ValidarProdutoAsync(produto))
                    {
                        await _unitOfWork.RollbackAsync();
                        return Enumerable.Empty<ProdutoDto>();
                    }

                    if (await _produtoRepositorio.ExisteProdutoComNomeAsync(produto.Nome))
                    {
                        _notificador.Handle(new Notificacao($"Já existe um produto com o nome '{produto.Nome}'"));
                        await _unitOfWork.RollbackAsync();
                        return Enumerable.Empty<ProdutoDto>();
                    }

                    var produtoCriado = await _produtoRepositorio.InserirAsync(produto);
                    if (produtoCriado != null)
                    {
                        produtosCriados.Add(_mapper.Map<ProdutoDto>(produtoCriado));
                    }
                }

                if (!await _unitOfWork.SalvarAlteracoesAsync())
                {
                    await _unitOfWork.RollbackAsync();
                    return Enumerable.Empty<ProdutoDto>();
                }

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao inserir produtos em lote: {ex.Message}"));
                await _unitOfWork.RollbackAsync();
                return Enumerable.Empty<ProdutoDto>();
            }

            return produtosCriados;
        }

        public async Task<bool> ExcluirVariosProdutosAsync(IEnumerable<int> ids)
        {
            if (ids == null)
            {
                _notificador.Handle(new Notificacao("Lista de IDs não pode ser nula"));
                return false;
            }

            var listaIds = ids.ToList();
            if (!listaIds.Any())
            {
                _notificador.Handle(new Notificacao("Lista de IDs não pode estar vazia"));
                return false;
            }

            if (listaIds.Any(id => id <= 0))
            {
                _notificador.Handle(new Notificacao("Todos os IDs devem ser maiores que zero"));
                return false;
            }

            await _unitOfWork.IniciarTransacaoAsync();

            try
            {
                var produtos = new List<Produto>();
                foreach (var id in listaIds)
                {
                    var produto = await _produtoRepositorio.ObterPorIdAsync(id);
                    if (produto != null)
                        produtos.Add(produto);
                }

                if (!produtos.Any())
                {
                    _notificador.Handle(new Notificacao("Nenhum produto foi encontrado com os IDs fornecidos"));
                    return false;
                }

                foreach (var produto in produtos)
                {
                    if (!await _produtoRepositorio.ExcluirAsync(produto))
                        return false;
                }

                if (!await _unitOfWork.SalvarAlteracoesAsync())
                    return false;

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao excluir produtos em lote: {ex.Message}"));
                return false;
            }
            finally
            {
                if (_unitOfWork.ExisteTransacaoAtiva)
                {
                    await _unitOfWork.RollbackAsync();
                }
            }
        }
        #endregion

        #region Métodos Privados
        private async Task<bool> ValidarProdutoAsync(Produto produto)
        {
            var validationResult = await _produtoValidator.ValidateAsync(produto);
            
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _notificador.Handle(new Notificacao(error.ErrorMessage));
                }
                return false;
            }

            return true;
        }
        #endregion
    }
}