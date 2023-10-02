using Microsoft.EntityFrameworkCore;
namespace CadastroDeProdutos
{
    public class CadProdutosDb : DbContext
    {
        public CadProdutosDb(DbContextOptions<CadProdutosDb> options) : base(options) { }

        public DbSet<CadProduto> CadProdutos => Set<CadProduto>();
    }
}
