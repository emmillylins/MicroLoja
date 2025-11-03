using FluentValidation;
using MicroLoja.ProdutoAPI.Dominio.Modelos;

namespace MicroLoja.ProdutoAPI.Aplicacao.Validacoes
{
    public class CategoriaValidacao : AbstractValidator<Categoria>
    {
        public CategoriaValidacao()
        {
            RuleFor(p => p.Nome)
                    .NotEmpty().WithMessage("O nome do produto é obrigatório.")
                    .NotNull().WithMessage("O nome do produto não pode ser nulo.")
                    .MaximumLength(100).WithMessage("O nome do produto deve ter no máximo 150 caracteres.")
                    .MinimumLength(2).WithMessage("O nome do produto deve ter pelo menos 2 caracteres.");

            RuleFor(p => p.Icone)
                    .NotEmpty().WithMessage("O icone produto é obrigatório.")
                    .NotNull().WithMessage("O icone do produto não pode ser nulo.")
                    .MaximumLength(100).WithMessage("O icone do produto deve ter no máximo 150 caracteres.")
                    .MinimumLength(2).WithMessage("O icone do produto deve ter pelo menos 2 caracteres.");
        }
    }
}
