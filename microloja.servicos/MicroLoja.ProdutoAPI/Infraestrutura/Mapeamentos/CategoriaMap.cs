using MicroLoja.ProdutoAPI.Dominio.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroLoja.ProdutoAPI.Infraestrutura.Mapeamentos
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("categorias")
                .HasKey(k => k.Id);

            builder.Property(k => k.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Nome)
                .HasColumnName("nome")
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(p => p.Icone)
                .HasColumnName("icone")
                .IsRequired(false)
                .HasColumnType("varchar(100)");
        }
    }
}
