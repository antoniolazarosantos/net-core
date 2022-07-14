using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();

app.MapGet("/", () => "Alô Mundão!");

app.MapPost("/user", () => new{Nome = "Antônio Lázaro" , Idade = 45} );

app.MapGet("/AddHeader",(HttpResponse response) => {
    response.Headers.Add("Nome","Antonio Lazaro");
    return new{Nome = "Antônio Lázaro" , Idade = 45};
}
);

app.MapPost("/produto",(Produto produto) => {
    ProdutoRepositorio.add(produto);
    return Results.Created($"/produto/{produto.Codigo}",produto.Codigo);
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



public static class ProdutoRepositorio{
    public static List<Produto> ListaProdutos {get;set;} =  ListaProdutos = new List<Produto>();

    public static void Init(IConfiguration conf){
        var lista = conf.GetSection("Carga_de_Produtos").Get<List<Produto>>();
        ListaProdutos = lista;

    }
    public static void add(Produto p) {
        if (ListaProdutos == null) {
            ListaProdutos = new List<Produto>();
        }
        ListaProdutos.Add(p);
    }

    public static Produto GetBy(string codigo){
        return ListaProdutos.FirstOrDefault(x => x.Codigo == codigo);
    }

    public static void Remover(Produto produto){
        ListaProdutos.Remove(produto);
    }
}

public class Tag {
    public int Id { get; set; }
    public string Nome { get; set; }

    public int ProdutoId {get; set;}
}
public class Categoria{
    public int Id { get; set; }
    public  string Nome { get; set; }
}
public class Produto {

    public int Id { get; set; }
    public string Codigo { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }

    public int CategoriaId { get; set; }
    public  Categoria Categoria { get; set; }

    public List<Tag> Tags { get; set;}
}

public class ApplicationDbContext : DbContext{
    
    public DbSet<Produto> Produtos {get; set;}

    protected override void  OnModelCreating(ModelBuilder builder){
        //FluentAPI
        builder.Entity<Produto>().Property(p => p.Descricao).HasMaxLength(500).IsRequired(false);
        builder.Entity<Produto>().Property(p => p.Nome).HasMaxLength(120).IsRequired();
        builder.Entity<Produto>().Property(p => p.Codigo).HasMaxLength(20).IsRequired();
        builder.Entity<Categoria>().Property(p => p.Nome).HasMaxLength(20).IsRequired();
         builder.Entity<Tag>().Property(p => p.Nome).HasMaxLength(20).IsRequired();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlServer("Server=localhost;Database=BDProduto;User Id=sa;Password=@Sql2019;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES");
}