using MicroLoja.ProdutoAPI.Dominio.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroLoja.ProdutoAPI.Infraestrutura.Mapeamentos
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("produtos")
                .HasKey(k => k.Id);

            builder.Property(k => k.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Nome)
                .HasColumnName("nome")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(p => p.Descricao)
                .HasColumnName("descricao")
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(p => p.ImagemUrl)
                .HasColumnName("imagem_url")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(p => p.Preco)
                .HasColumnName("preco")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.CategoriaId)
                .HasColumnName("categoria_id")
                .IsRequired(false);

            builder.HasOne(p => p.Categoria)
                .WithMany(p => p.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .IsRequired(false);

            builder.HasIndex(p => p.CategoriaId)
                .IsUnique(false);
        }
    }
}
