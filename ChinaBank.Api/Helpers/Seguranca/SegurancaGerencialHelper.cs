using Microsoft.AspNetCore.Http;

namespace ChinaBank.Api.Helpers
{
    public class SegurancaGerencialHelper
    {
        public static bool Autorizado(HttpRequest req)
        {
            bool ret = false;
            string usuario = req.Headers[Rotas.Usuario_http_header].ToString();
            string senha = req.Headers[Rotas.Senha_http_header].ToString();

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrWhiteSpace(usuario) ||
                  string.IsNullOrEmpty(senha) || string.IsNullOrWhiteSpace(senha))
            {
                ret = false;
            }
            else
            {
                if (usuario.Equals("admin") && senha.Equals("admin")) {
                    ret = true;
                }                
            }

            return ret;
        }
    }
}
