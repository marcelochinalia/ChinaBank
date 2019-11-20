using System.Collections.Generic;

namespace ChinaBank.Api.Models
{
    public class ContaPaginado
    {
        public ContaFiltro Filtro { get; set; }
        public string PaginaAnterior { get; set; }
        public string ProximaPagina { get; set; }
        public List<Conta> Contas { get; set; }

        public ContaPaginado(ContaFiltro filtro)
        {
            Filtro = filtro;
            Contas = new List<Conta>();
        }
    }
}
