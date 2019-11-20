namespace ChinaBank.Api.Models
{
    public class ContaFiltro
    {
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }

        public ContaFiltro(int pagina, int tamanhoPagina)
        {
            Pagina = pagina == 0 ? 1 : pagina;
            TamanhoPagina = tamanhoPagina == 0 || tamanhoPagina > 100 ? 50 : tamanhoPagina;
        }        
    }
}
