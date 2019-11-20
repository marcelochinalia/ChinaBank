using Security.Api.Models;

public interface IAcessoRepository
{
    string ObterToken(Acesso acesso);
    ContaCriada CriarContaSegura();
    bool ValidarToken(string token, string numeroConta);
}