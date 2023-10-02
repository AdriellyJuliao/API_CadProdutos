using Microsoft.EntityFrameworkCore;
using CadastroDeProdutos;

namespace CadastroDeProdutos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<CadProdutosDb>(opt => opt.UseInMemoryDatabase("CadastroDeProdutos"));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            var app = builder.Build();

            //FAZ A LEITURA DOS DADOS DO BANCO DE DADOS COMPLETO
            app.MapGet("/CadItems", async (CadProdutosDb db) =>
            await db.CadProdutos.ToListAsync());


            //FAZ A LEITURA DOS DADOS MEDIANTE AO CODIGO DO PRODUTO
            /*app.MapGet("/CadItems/{codigoProduto}", async (CadProdutosDb db, int codigoProduto) =>
            { await db.CadProdutos.FirstOrDefaultAsync(p => p.CodigoProduto == codigoProduto)    //FindAsync -> procura dentro do banco se aquela variavel existe
                is CadProduto cadastro
                ? Results.Ok(cadastro)
                : Results.NotFound()
                });*/

            //FAZ A LEITURA DOS DADOS MEDIANTE AO CODIGO DO PRODUTO
            app.MapGet("/CadItems/{codigoProduto}", async (CadProdutosDb db, int codigoProduto) =>
            {
                var CodProduto = await db.CadProdutos
                .Where(p => p.CodigoProduto == codigoProduto).ToListAsync();
                if (CodProduto.Any()){
                    return Results.Ok(CodProduto);
                }else{
                    return Results.NotFound();
                }
            });

            //FAZ A LEITURA DOS DADOS MEDIANTE A CATEGORIA
            /*app.MapGet("/CadItems/categoria/{categoria}", async (CadProdutosDb db, string categoria) =>
            await db.CadProdutos.FindAsync()    //FindAsync -> procura dentro do banco se aquela variavel existe
                is CadProduto cadastro
                ? Results.Ok(cadastro)
                : Results.NotFound());*/

            //FAZ A LEITURA DOS DADOS MEDIANTE A CATEGORIA
            app.MapGet("/CadItems/categoria/{categoria}", async (CadProdutosDb db, string categoria) =>
            {   var CategoriaProdutos = await db.CadProdutos.Where(p => p.Categoria == categoria).ToListAsync();

                if (CategoriaProdutos.Any()){
                    return Results.Ok(CategoriaProdutos);
                }else{
                    return Results.NotFound();
                }
            });



            //FAZ A CRIAÇÃO(POST) DOS DADOS 
            app.MapPost("/CadItems", async (CadProduto cadproduto, CadProdutosDb db) =>
            {
                db.CadProdutos.Add(cadproduto);
                await db.SaveChangesAsync();

                return Results.Created($"/todoitems/{cadproduto.Id}", cadproduto);
            });

            //MODIFICA OS DADOS DO BANCO DE DADOS
            app.MapPut("/CadItems/{id}", async (CadProdutosDb db, int id, CadProduto inputCadProdtos) =>
            {
                var items = await db.CadProdutos.FindAsync(id);

                if (items is null) Results.NotFound();

                items.NomeProduto = inputCadProdtos.NomeProduto;
                items.CodigoProduto = inputCadProdtos.CodigoProduto;
                items.Preco = inputCadProdtos.Preco;
                items.Descricao = inputCadProdtos.Descricao;
                items.QuantidadeEstoque = inputCadProdtos.QuantidadeEstoque;
                items.Avaliacao = inputCadProdtos.Avaliacao;
                items.Categoria = inputCadProdtos.Categoria;
                

                await db.SaveChangesAsync();

                return Results.NoContent();
            }
            );

            //DELETA OS DADOS DO BANCO DE DADOS
            app.MapDelete("/CadItems/{id}", async (CadProdutosDb db, int id) =>
            {
                if (await db.CadProdutos.FindAsync(id) is CadProduto cadproduto)
                {
                    db.CadProdutos.Remove(cadproduto);
                    await db.SaveChangesAsync();
                    return Results.Ok(cadproduto);
                }
                else
                {
                    return Results.NotFound();
                }
            });

            app.Urls.Add("https://localhost:3000");

            app.Run();
        }
    }
}