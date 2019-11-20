using System.Collections.Generic;

namespace ChinaBank.Api.Models
{
    public class ExtratoPaginado
    {
        public ExtratoFiltro Filtro { get; set; }
        public string PaginaAnterior { get; set; }
        public string ProximaPagina { get; set; }
        public List<Extrato> Extratos { get; set; }

        public ExtratoPaginado(ExtratoFiltro filtro)
        {
            Filtro = filtro;
            Extratos = new List<Extrato>();
        }
    }
}
