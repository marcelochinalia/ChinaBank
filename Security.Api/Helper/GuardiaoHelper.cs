using Security.Api.Models;

namespace Security.Api.Helper
{
    public class GuardiaoHelper
    {
        private readonly IAcessoRepository _repo;

        public GuardiaoHelper(IAcessoRepository repo)
        {
            _repo = repo;
        }

        private bool Validado(Acesso acesso)
        {
            bool ret = true;

            if (acesso != null)
            {
                if (acesso.NumeroConta == null || acesso.NumeroConta.Trim().Length == 0)
                {
                    return false;
                }

                if (acesso.Senha == null || acesso.Senha.Trim().Length == 0)
                {
                    return false;
                }
            }

            return ret;
        }

        public string ObterToken(Acesso acesso)
        {
            string token = null;

            if (Validado(acesso))
            {
                token = _repo.ObterToken(acesso);
            }

            return token;
        }

        public ContaCriada CriarContaSegura()
        {
            return _repo.CriarContaSegura();
        }
    }    
}
