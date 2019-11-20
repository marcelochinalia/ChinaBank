using ChinaBank.Api.Models;

public interface IContaRepository
{
    bool ContaExiste(string numeroConta);
    bool Depositar(Deposito deposito);    
    Saldo ObterSaldo(string numeroConta);    
    ExtratoPaginado ObterExtrato(ExtratoFiltro filtro);
    bool Transferir(Transferencia transferencia);
}

