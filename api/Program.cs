using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Alô Mundão!");

app.MapPost("/user", () => new{Nome = "Antônio Lázaro" , Idade = 45} );

app.MapGet("/AddHeader",(HttpResponse response) => {
    response.Headers.Add("Nome","Antonio Lazaro");
    return new{Nome = "Antônio Lázaro" , Idade = 45};
}
);

app.MapPost("/salvarproduto",(Produto produto) => {
    ProdutoRepositorio.add(produto);
});

app.MapPut("/atualizarproduto",(Produto produto) => {
    var p = ProdutoRepositorio.GetBy(produto.Codigo);
    p.Nome = produto.Nome;
});

app.MapDelete("/apagarproduto/{codigo}",([FromRoute] string codigo) => {
    var registro = ProdutoRepositorio.GetBy(codigo);
    ProdutoRepositorio.Remover(registro);
});

app.MapGet("/getproduto",([FromQuery] string dataInicial, [FromQuery] string dataFinal) => {
    return dataInicial + " - " + dataFinal;
});

app.MapGet("/getproduto/{codigo}",([FromRoute] string codigo) => {
    var registro = ProdutoRepositorio.GetBy(codigo);
    return registro;
});

app.MapGet("/getprodutoheader",(HttpRequest request) => {
    return request.Headers["produto-codigo"].ToString();
});


app.Run();


public static class ProdutoRepositorio{
    public static List<Produto> ListaProdutos {get;set;}
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
public class Produto {
    public string Codigo { get; set; }
    public string Nome { get; set; }
}