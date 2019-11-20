public static class Rotas
{
    public static string _BaseUrlChinaBankApi = "https://localhost:44315";

    public static string _ListagemContas = "/api/admin/contas?pagina={0}&tamanhopagina={1}";
    public static string _DetalhesCliente = "/api/admin/cliente/{0}";
    public static string _ListagemMovimentacoesConta = "/api/admin/{0}/movimentacoes?pagina={1}&tamanhopagina={2}";

    public static string _Usuario = "admin";
    public static string _Senha = "admin";
}

