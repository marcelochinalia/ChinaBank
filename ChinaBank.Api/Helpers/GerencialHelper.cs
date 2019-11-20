using ChinaBank.Api.Models;

namespace ChinaBank.Api.Helpers.Admin
{
    public class GerencialHelper
    {
        private readonly IGerencialRepository _repo;

        public GerencialHelper(IGerencialRepository repo)
        {
            _repo = repo;
        }
        public ContaPaginado ObterContas(int pagina, int tamanhoPagina)
        {
            return _repo.ObterContas(new ContaFiltro(pagina, tamanhoPagina));
        }

        public ExtratoPaginado ObterMovimentacoes(string numeroConta, int pagina, int tamanhoPagina)
        {
            return _repo.ObterMovimentacoes(new ExtratoFiltro(numeroConta, 60, pagina, tamanhoPagina));
        }        
    }
}
