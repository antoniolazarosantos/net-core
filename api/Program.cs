using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();

app.MapGet("/", () => "Alô Mundão!");

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
    if(produtoRequest.Tags != null){
        prod.Tags = new List<Tag>();
        foreach(var item in produtoRequest.Tags){
            prod.Tags.Add(new Tag{Nome = item});
        }
    }
    contexto.Produtos.Add(prod);
    contexto.SaveChanges();
    return Results.Created($"/produto/{prod.Id}",prod.Id);
});

app.MapPut("/produto",(Produto produto) => {
    var p = ProdutoRepositorio.GetBy(produto.Codigo);
    p.Nome = produto.Nome;
    return Results.Ok();
});

app.MapDelete("/produto/{codigo}",([FromRoute] string codigo) => {
    var registro = ProdutoRepositorio.GetBy(codigo);
    ProdutoRepositorio.Remover(registro);
    return Results.Ok();
});

app.MapGet("/produto",([FromQuery] string dataInicial, [FromQuery] string dataFinal) => {
    return dataInicial + " - " + dataFinal;
});

app.MapGet("/produto/{codigo}",([FromRoute] string codigo) => {
    var registro = ProdutoRepositorio.GetBy(codigo);
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
