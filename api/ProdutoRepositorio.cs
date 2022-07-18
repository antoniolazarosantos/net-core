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
