using Microsoft.AspNetCore.Mvc;
using MicroLoja.ProdutoAPI.Aplicacao.DTOs;
using MicroLoja.ProdutoAPI.Aplicacao.Interfaces;
using MicroLoja.ProdutoAPI.Controllers.Main;
using MicroLoja.ProdutoAPI.Dominio.Notificacoes;

namespace MicroLoja.ProdutoAPI.Controllers
{
    [Route("api/produtos")]
    public class ProdutoController : MainController
    {
        private readonly IProdutoServico _produtoServico;

        public ProdutoController(INotificador notificador, IProdutoServico produtoServico) 
            : base(notificador)
        {
            _produtoServico = produtoServico;
        }

        #region CRUD Básico
        /// <summary>
        /// Obtém um produto por ID
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <returns>Produto encontrado ou erro se não existir</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult> ObterPorId(int id)
        {
            var produto = await _produtoServico.ObterPorIdAsync(id);
            return RespostaPersonalizada(produto);
        }

        /// <summary>
        /// Obtém todos os produtos
        /// </summary>
        /// <returns>Lista de produtos</returns>
        [HttpGet]
        public async Task<ActionResult> ObterTodos()
        {
            var produtos = await _produtoServico.ObterTodosAsync();
            return RespostaPersonalizada(produtos);
        }

        /// <summary>
        /// Cria um novo produto
        /// </summary>
        /// <param name="produto">Dados do produto</param>
        /// <returns>Produto criado ou erros de validação</returns>
        [HttpPost]
        public async Task<ActionResult> Criar([FromBody] CriarAtualizarProdutoDto produto)
        {
            if (!ModelState.IsValid) 
                return RespostaPersonalizada(ModelState);

            var produtoCriado = await _produtoServico.InserirProdutoAsync(produto);
            return RespostaPersonalizada(produtoCriado);
        }

        /// <summary>
        /// Atualiza um produto existente
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <param name="produto">Dados atualizados do produto</param>
        /// <returns>Produto atualizado ou erros de validação</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Atualizar(int id, [FromBody] CriarAtualizarProdutoDto produto)
        {
            if (!ModelState.IsValid) 
                return RespostaPersonalizada(ModelState);

            var produtoAtualizado = await _produtoServico.AtualizarProdutoAsync(id, produto);
            return RespostaPersonalizada(produtoAtualizado);
        }

        /// <summary>
        /// Exclui um produto
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <returns>Sucesso ou erro se não encontrado</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Excluir(int id)
        {
            var resultado = await _produtoServico.ExcluirProdutoAsync(id);
            return RespostaPersonalizada(resultado);
        }
        #endregion

        #region Consultas Específicas
        /// <summary>
        /// Obtém produtos por categoria
        /// </summary>
        /// <param name="categoriaId">ID da categoria</param>
        /// <returns>Lista de produtos da categoria</returns>
        [HttpGet("categoria/{categoriaId:int}")]
        public async Task<ActionResult> ObterPorCategoria(int categoriaId)
        {
            var produtos = await _produtoServico.ObterProdutosPorCategoriaAsync(categoriaId);
            return RespostaPersonalizada(produtos);
        }

        /// <summary>
        /// Busca produtos por nome
        /// </summary>
        /// <param name="nome">Nome ou parte do nome do produto</param>
        /// <returns>Lista de produtos encontrados</returns>
        [HttpGet("buscar")]
        public async Task<ActionResult> BuscarPorNome([FromQuery] string nome)
        {
            var produtos = await _produtoServico.BuscarPorNomeAsync(nome);
            return RespostaPersonalizada(produtos);
        }

        /// <summary>
        /// Obtém produtos por faixa de preço
        /// </summary>
        /// <param name="precoMinimo">Preço mínimo</param>
        /// <param name="precoMaximo">Preço máximo</param>
        /// <returns>Lista de produtos na faixa de preço</returns>
        [HttpGet("preco")]
        public async Task<ActionResult> ObterPorFaixaPreco([FromQuery] decimal precoMinimo, [FromQuery] decimal precoMaximo)
        {
            var produtos = await _produtoServico.ObterProdutosPorFaixaPrecoAsync(precoMinimo, precoMaximo);
            return RespostaPersonalizada(produtos);
        }

        /// <summary>
        /// Obtém os produtos mais caros
        /// </summary>
        /// <param name="quantidade">Quantidade de produtos a retornar (padrão: 10)</param>
        /// <returns>Lista dos produtos mais caros</returns>
        [HttpGet("mais-caros")]
        public async Task<ActionResult> ObterMaisCaros([FromQuery] int quantidade = 10)
        {
            var produtos = await _produtoServico.ObterProdutosMaisCarosAsync(quantidade);
            return RespostaPersonalizada(produtos);
        }

        /// <summary>
        /// Obtém os produtos mais baratos
        /// </summary>
        /// <param name="quantidade">Quantidade de produtos a retornar (padrão: 10)</param>
        /// <returns>Lista dos produtos mais baratos</returns>
        [HttpGet("mais-baratos")]
        public async Task<ActionResult> ObterMaisBaratos([FromQuery] int quantidade = 10)
        {
            var produtos = await _produtoServico.ObterProdutosMaisBaratosAsync(quantidade);
            return RespostaPersonalizada(produtos);
        }
        #endregion

        #region Validações e Verificações
        /// <summary>
        /// Verifica se um produto existe
        /// </summary>
        /// <param name="id">ID do produto</param>
        /// <returns>True se existe, False caso contrário</returns>
        [HttpGet("{id:int}/existe")]
        public async Task<ActionResult> VerificarExistencia(int id)
        {
            var existe = await _produtoServico.ExisteProdutoAsync(id);
            return RespostaPersonalizada(existe);
        }

        /// <summary>
        /// Verifica se existe produto com nome específico
        /// </summary>
        /// <param name="nome">Nome do produto</param>
        /// <param name="excluirId">ID do produto a excluir da verificação (opcional)</param>
        /// <returns>True se existe, False caso contrário</returns>
        [HttpGet("existe-nome")]
        public async Task<ActionResult> VerificarExistenciaPorNome([FromQuery] string nome, [FromQuery] int? excluirId = null)
        {
            var existe = await _produtoServico.ExisteProdutoComNomeAsync(nome, excluirId);
            return RespostaPersonalizada(existe);
        }

        /// <summary>
        /// Conta o total de produtos
        /// </summary>
        /// <returns>Número total de produtos</returns>
        [HttpGet("contar")]
        public async Task<ActionResult> ContarProdutos()
        {
            var total = await _produtoServico.ContarProdutosAsync();
            return RespostaPersonalizada(total);
        }

        /// <summary>
        /// Conta produtos por categoria
        /// </summary>
        /// <param name="categoriaId">ID da categoria</param>
        /// <returns>Número de produtos na categoria</returns>
        [HttpGet("contar/categoria/{categoriaId:int}")]
        public async Task<ActionResult> ContarPorCategoria(int categoriaId)
        {
            var total = await _produtoServico.ContarProdutosPorCategoriaAsync(categoriaId);
            return RespostaPersonalizada(total);
        }
        #endregion

        #region Operações em Lote
        /// <summary>
        /// Cria múltiplos produtos
        /// </summary>
        /// <param name="produtos">Lista de produtos a criar</param>
        /// <returns>Lista de produtos criados ou erros de validação</returns>
        [HttpPost("lote")]
        public async Task<ActionResult> CriarVarios([FromBody] IEnumerable<CriarAtualizarProdutoDto> produtos)
        {
            if (!ModelState.IsValid) 
                return RespostaPersonalizada(ModelState);

            var produtosCriados = await _produtoServico.InserirVariosProdutosAsync(produtos);
            return RespostaPersonalizada(produtosCriados);
        }

        /// <summary>
        /// Exclui múltiplos produtos
        /// </summary>
        /// <param name="ids">Lista de IDs dos produtos a excluir</param>
        /// <returns>Sucesso ou erros se algum produto não for encontrado</returns>
        [HttpDelete("lote")]
        public async Task<ActionResult> ExcluirVarios([FromBody] IEnumerable<int> ids)
        {
            if (!ModelState.IsValid) 
                return RespostaPersonalizada(ModelState);

            var resultado = await _produtoServico.ExcluirVariosProdutosAsync(ids);
            return RespostaPersonalizada(resultado);
        }
        #endregion
    }
}
