namespace ChinaBank.Api.Models
{
    public class Extrato
    {
        public int Id { get; set; }
        public string NumeroConta { get; set; }
        public long DataOperacao { get; set; }
        public double ValorOperacao { get; set; }
        public string TipoOperacao { get; set; }
        public string Transacao { get; set; }
    }
}
