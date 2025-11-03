using AutoMapper;
using FluentValidation;
using MicroLoja.ProdutoAPI.Aplicacao.DTOs;
using MicroLoja.ProdutoAPI.Aplicacao.Interfaces;
using MicroLoja.ProdutoAPI.Dominio.Interfaces;
using MicroLoja.ProdutoAPI.Dominio.Interfaces.Base;
using MicroLoja.ProdutoAPI.Dominio.Modelos;
using MicroLoja.ProdutoAPI.Dominio.Notificacoes;

namespace MicroLoja.ProdutoAPI.Aplicacao.Servicos
{
    public class CategoriaServico : ICategoriaServico
    {
        private readonly IRepositorioBase<Categoria> _categoriaRepositorio;
        private readonly INotificador _notificador;
        private readonly IValidator<Categoria> _categoriaValidator;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaServico(IRepositorioBase<Categoria> categoriaRepositorio,
                               INotificador notificador,
                               IValidator<Categoria> categoriaValidator,
                               IMapper mapper,
                               IUnitOfWork unitOfWork)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _notificador = notificador;
            _categoriaValidator = categoriaValidator;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoriaDto>> ObterTodasCategoriasAsync()
        {
            var categorias = await _categoriaRepositorio.ObterTodosAsync();
            return _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
        }

        public async Task<CategoriaDto?> ObterCategoriaPorIdAsync(int id)
        {
            if (id <= 0)
            {
                _notificador.Handle(new Notificacao("ID deve ser maior que zero"));
                return null;
            }

            var categoria = await _categoriaRepositorio.ObterPorIdAsync(id);
            return categoria != null ? _mapper.Map<CategoriaDto>(categoria) : null;
        }

        public async Task<CategoriaDto?> InserirCategoriaAsync(CriarAtualizarCategoriaDto categoriaDto)
        {
            if (categoriaDto == null)
            {
                _notificador.Handle(new Notificacao("Categoria não pode ser nula"));
                return null;
            }

            await _unitOfWork.IniciarTransacaoAsync();

            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);

                if (!await ValidarCategoriaAsync(categoria))
                    return null;

                if (await ExisteCategoriaComNomeAsync(categoria.Nome))
                {
                    _notificador.Handle(new Notificacao($"Já existe uma categoria com o nome '{categoria.Nome}'"));
                    return null;
                }

                var categoriaCriada = await _categoriaRepositorio.InserirAsync(categoria);
                if (categoriaCriada == null)
                    return null;

                if (!await _unitOfWork.SalvarAlteracoesAsync())
                    return null;

                await _unitOfWork.CommitAsync();

                return _mapper.Map<CategoriaDto>(categoriaCriada);
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao adicionar categoria: {ex.Message}"));
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

        public async Task<bool> AtualizarCategoriaAsync(int id, CriarAtualizarCategoriaDto categoriaDto)
        {
            if (id <= 0)
            {
                _notificador.Handle(new Notificacao("ID deve ser maior que zero"));
                return false;
            }

            if (categoriaDto == null)
            {
                _notificador.Handle(new Notificacao("Categoria não pode ser nula"));
                return false;
            }

            await _unitOfWork.IniciarTransacaoAsync();

            try
            {
                var categoriaExistente = await _categoriaRepositorio.ObterPorIdAsync(id);
                if (categoriaExistente == null)
                {
                    _notificador.Handle(new Notificacao("Categoria não encontrada"));
                    return false;
                }

                _mapper.Map(categoriaDto, categoriaExistente);

                if (!await ValidarCategoriaAsync(categoriaExistente))
                    return false;

                if (await ExisteCategoriaComNomeAsync(categoriaExistente.Nome, id))
                {
                    _notificador.Handle(new Notificacao($"Já existe outra categoria com o nome '{categoriaExistente.Nome}'"));
                    return false;
                }

                if (!await _categoriaRepositorio.AtualizarAsync(categoriaExistente))
                    return false;

                if (!await _unitOfWork.SalvarAlteracoesAsync())
                    return false;

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao atualizar categoria: {ex.Message}"));
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

        public async Task<bool> ExcluirCategoriaAsync(int id)
        {
            if (id <= 0)
            {
                _notificador.Handle(new Notificacao("ID deve ser maior que zero"));
                return false;
            }

            await _unitOfWork.IniciarTransacaoAsync();

            try
            {
                var categoria = await _categoriaRepositorio.ObterPorIdAsync(id);
                if (categoria == null)
                {
                    _notificador.Handle(new Notificacao($"Categoria com ID {id} não encontrada"));
                    return false;
                }

                // Verificar se existem produtos vinculados à categoria
                var possuiProdutos = await _categoriaRepositorio.ExisteAsync(c => c.Id == id && c.Produtos.Any());
                if (possuiProdutos)
                {
                    _notificador.Handle(new Notificacao("Não é possível remover uma categoria que possui produtos vinculados"));
                    return false;
                }

                if (!await _categoriaRepositorio.ExcluirAsync(categoria))
                    return false;

                if (!await _unitOfWork.SalvarAlteracoesAsync())
                    return false;

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _notificador.Handle(new Notificacao($"Erro ao remover categoria: {ex.Message}"));
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

        #region Métodos Privados
        private async Task<bool> ValidarCategoriaAsync(Categoria categoria)
        {
            var validationResult = await _categoriaValidator.ValidateAsync(categoria);

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

        private async Task<bool> ExisteCategoriaComNomeAsync(string nome, int? excluirId = null)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return false;

            if (excluirId.HasValue)
            {
                return await _categoriaRepositorio.ExisteAsync(c => c.Nome == nome && c.Id != excluirId.Value);
            }

            return await _categoriaRepositorio.ExisteAsync(c => c.Nome == nome);
        }
        #endregion
    }
}
