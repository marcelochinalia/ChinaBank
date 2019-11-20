public class TokenHelper
{
    private readonly IAcessoRepository _repo;

    public TokenHelper(IAcessoRepository repo)
    {
        _repo = repo;
    }

    public bool Autorizado(string token, string numeroConta)
    {
        bool ret = false;

        ret = _repo.ValidarToken(token, numeroConta);

        return ret;
    }
}