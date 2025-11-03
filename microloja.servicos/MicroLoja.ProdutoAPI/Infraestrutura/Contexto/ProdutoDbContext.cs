using MicroLoja.ProdutoAPI.Dominio.Modelos;
using MicroLoja.ProdutoAPI.Infraestrutura.Seed;
using Microsoft.EntityFrameworkCore;

namespace MicroLoja.ProdutoAPI.Infraestrutura.Contexto
{
    public class ProdutoDbContext : DbContext
    {
        public ProdutoDbContext() { }
        public ProdutoDbContext(DbContextOptions<ProdutoDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica todos os mapeamentos do assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProdutoDbContext).Assembly);

            ProdutoSeed.Popular(modelBuilder);
        }
    }
}
