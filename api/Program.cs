using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();

app.MapGet("/", () => "Serviço rodando.");

app.MapPost("/user", () => new{Nome = "Antônio Lázaro" , Idade = 45} );

app.MapGet("/AddHeader",(HttpResponse response) => {
    response.Headers.Add("Nome","Antonio Lazaro");
    return new{Nome = "Antônio Lázaro" , Idade = 45};
}
);

app.MapPost("/produto",(ProdutoRequest produtoRequest, ApplicationDbContext contexto) => {
    var cat = contexto.Categorias.Where(a => a.Id == produtoRequest.CategoriaId).First();
    var prod = new Produto{
        Codigo = produtoRequest.Codigo,
        Nome = produtoRequest.Nome,
        Descricao = produtoRequest.Descricao,
        Categoria = cat
    };
    contexto.Produtos.Add(prod);
    contexto.SaveChanges();
    return Results.Created($"/produto/{prod.Id}",prod.Id);
});

app.MapPut("/produto/{id}",([FromRoute] int id,ProdutoRequest produtoRequest,ApplicationDbContext contexto) => {
    var registro = contexto.Produtos
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();
    var cat = contexto.Categorias.Where(a => a.Id == produtoRequest.CategoriaId).First();
    registro.Codigo = produtoRequest.Codigo;
    registro.Nome = produtoRequest.Nome;
    registro.Descricao = produtoRequest.Descricao;
    registro.Categoria = cat;
    if(produtoRequest.Tags != null){
        registro.Tags = new List<Tag>();
        foreach(var item in produtoRequest.Tags){
            registro.Tags.Add(new Tag{Nome = item});
        }
    }
    contexto.SaveChanges();
    return Results.Ok();
});

app.MapDelete("/produto/{id}",([FromRoute] int id,ApplicationDbContext contexto) => {
    var registro = contexto.Produtos.Where(p => p.Id == id).First();
    contexto.Remove(registro);
    contexto.SaveChanges();
    return Results.Ok();
});

app.MapGet("/produto",([FromQuery] string dataInicial, [FromQuery] string dataFinal) => {
    return dataInicial + " - " + dataFinal;
});

app.MapGet("/produto/{id}",([FromRoute] int id,ApplicationDbContext contexto) => {
    var registro = contexto.Produtos
    .Include(p => p.Categoria)
    .Include(p => p.Tags)
    .Where(p => p.Id == id).First();
    if (registro != null){
      return Results.Ok(registro);
    }
    return Results.NotFound();
});

app.MapGet("/getprodutoheader",(HttpRequest request) => {
    return request.Headers["produto-codigo"].ToString();
});

if(app.Environment.IsStaging())
app.MapGet("/configuracao/database",(IConfiguration configuracao) => {
 return Results.Ok($"{configuracao["Database:Connection"]}:{configuracao["Database:Porta"]}");
});

var c = app.Configuration;
ProdutoRepositorio.Init(c);

app.Run();
