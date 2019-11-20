internal static class Rotas 
{
    public const string BaseURL = "https://localhost:44308";

    public const string ObterToken = "/api/guarda";
    public const string ValidarToken = "/api/guarda/validate";
    public const string CriarConta = "/api/guarda/create";

    public const string Conta_http_header = "numeroconta";
    public const string Token_http_header = "token";

    public const string Usuario_http_header = "usuario";
    public const string Senha_http_header = "senha";

    public const int HttpStatusCodeIntervalError = 500;
}