namespace ChinaBank.Api.Models
{
    public class Conta : Saldo
    {
        public string Cliente { get; set; }
        public string Extrato { get; set; }
    }
}
