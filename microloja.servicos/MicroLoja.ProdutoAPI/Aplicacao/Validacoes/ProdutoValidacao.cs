using FluentValidation;
using MicroLoja.ProdutoAPI.Dominio.Modelos;

namespace MicroLoja.ProdutoAPI.Aplicacao.Validacoes
{
    public class ProdutoValidacao : AbstractValidator<Produto>
    {
        public ProdutoValidacao()
        {
            RuleFor(p => p.Nome)
                .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                .NotNull().WithMessage("O nome do produto não pode ser nulo.")
                .MaximumLength(150).WithMessage("O nome do produto deve ter no máximo 150 caracteres.")
                .MinimumLength(2).WithMessage("O nome do produto deve ter pelo menos 2 caracteres.");

            RuleFor(p => p.Preco)
                .GreaterThan(0).WithMessage("O preço do produto deve ser maior que zero.")
                .NotNull().WithMessage("O preço do produto é obrigatório.");

            RuleFor(p => p.Descricao)
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.");

            RuleFor(p => p.CategoriaId)
                .NotNull().WithMessage("A categoria do produto é obrigatória.")
                .GreaterThan(0).WithMessage("O ID da categoria deve ser maior que zero.");

            RuleFor(p => p.ImagemUrl)
                .MaximumLength(500).WithMessage("A URL da imagem deve ter no máximo 500 caracteres.")
                //.Must(UrlValida).WithMessage("A URL da imagem deve ser válida.")
                .When(p => !string.IsNullOrEmpty(p.ImagemUrl));
        }

        private static bool UrlValida(string? url)
        {
            if (string.IsNullOrEmpty(url)) return true; // URL é opcional
            
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
