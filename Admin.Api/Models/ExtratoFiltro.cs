namespace Admin.Api.Models
{
    public class ExtratoFiltro
    {
        public string NumeroConta { get; set; }
        public int UltimosDias { get; set; }
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }

        public ExtratoFiltro(string numeroConta, int ultimosDias, int pagina, int tamanhoPagina)
        {
            NumeroConta = numeroConta;
            UltimosDias = ultimosDias == 0 ? 7 : ultimosDias;
            Pagina = pagina == 0 ? 1 : pagina;
            TamanhoPagina = tamanhoPagina == 0 || tamanhoPagina > 100 ? 50 : tamanhoPagina;
        }        
    }
}
