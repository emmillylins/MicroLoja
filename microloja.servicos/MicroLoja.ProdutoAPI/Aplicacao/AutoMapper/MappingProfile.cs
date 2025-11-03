using AutoMapper;
using MicroLoja.ProdutoAPI.Aplicacao.DTOs;
using MicroLoja.ProdutoAPI.Dominio.Modelos;

namespace MicroLoja.ProdutoAPI.Aplicacao.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Produto, ProdutoDto>()
               .ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria));

            CreateMap<CriarAtualizarProdutoDto, Produto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Categoria, opt => opt.Ignore());

            CreateMap<Categoria, CategoriaDto>();

        }
    }
}