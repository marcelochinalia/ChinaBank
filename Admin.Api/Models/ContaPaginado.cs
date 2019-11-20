using System.Collections.Generic;
using System.Text.Json;

namespace Admin.Api.Models
{
    public class ContaPaginado
    {
        public ContaFiltro Filtro { get; set; }
        public string PaginaAnterior { get; set; }
        public string ProximaPagina { get; set; }
        public List<Conta> Contas { get; set; }

        public static ContaPaginado FromJson(string obj)
        {
            return JsonSerializer.Deserialize<ContaPaginado>(obj);
        }
    }
}
