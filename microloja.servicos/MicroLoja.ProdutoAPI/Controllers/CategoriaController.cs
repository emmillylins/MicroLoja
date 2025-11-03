using Microsoft.AspNetCore.Mvc;
using MicroLoja.ProdutoAPI.Aplicacao.DTOs;
using MicroLoja.ProdutoAPI.Aplicacao.Interfaces;
using MicroLoja.ProdutoAPI.Controllers.Main;
using MicroLoja.ProdutoAPI.Dominio.Notificacoes;

namespace MicroLoja.ProdutoAPI.Controllers
{
    [Route("api/categorias")]
    public class CategoriaController : MainController
    {
        private readonly ICategoriaServico _categoriaServico;

        public CategoriaController(INotificador notificador, ICategoriaServico categoriaServico) 
            : base(notificador)
        {
            _categoriaServico = categoriaServico;
        }

        #region CRUD
        /// <summary>
        /// Obtém uma categoria por ID
        /// </summary>
        /// <param name="id">ID da categoria</param>
        /// <returns>Categoria encontrada ou erro se não existir</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObterPorId(int id)
        {
            var categoria = await _categoriaServico.ObterCategoriaPorIdAsync(id);
            return RespostaPersonalizada(categoria);
        }

        /// <summary>
        /// Obtém todas as categorias
        /// </summary>
        /// <returns>Lista de categorias</returns>
        [HttpGet]
        public async Task<ActionResult> ObterTodas()
        {
            var categorias = await _categoriaServico.ObterTodasCategoriasAsync();
            return RespostaPersonalizada(categorias);
        }

        /// <summary>
        /// Cria uma nova categoria
        /// </summary>
        /// <param name="categoria">Dados da categoria</param>
        /// <returns>Categoria criada ou erros de validação</returns>
        [HttpPost]
        public async Task<ActionResult> Criar([FromBody] CriarAtualizarCategoriaDto categoria)
        {
            if (!ModelState.IsValid) 
                return RespostaPersonalizada(ModelState);

            var categoriaCriada = await _categoriaServico.InserirCategoriaAsync(categoria);
            return RespostaPersonalizada(categoriaCriada);
        }

        /// <summary>
        /// Atualiza uma categoria existente
        /// </summary>
        /// <param name="id">ID da categoria</param>
        /// <param name="categoria">Dados atualizados da categoria</param>
        /// <returns>Sucesso ou erros de validação</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] CriarAtualizarCategoriaDto categoria)
        {
            if (!ModelState.IsValid) 
                return RespostaPersonalizada(ModelState);

            var resultado = await _categoriaServico.AtualizarCategoriaAsync(id, categoria);
            return RespostaPersonalizada(resultado);
        }

        /// <summary>
        /// Exclui uma categoria
        /// </summary>
        /// <param name="id">ID da categoria</param>
        /// <returns>Sucesso ou erro se não encontrada ou possui produtos vinculados</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Excluir(int id)
        {
            var resultado = await _categoriaServico.ExcluirCategoriaAsync(id);
            return RespostaPersonalizada(resultado);
        }
        #endregion
    }
}