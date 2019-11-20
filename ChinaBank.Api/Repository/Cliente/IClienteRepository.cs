using ChinaBank.Api.Models;

public interface IClienteRepository
{
    bool AbertaContaCliente(Cliente cliente, string numeroConta);
    bool Atualizar(Cliente cliente);
    bool ClienteExiste(int id);
    bool ClienteExiste(string documento);    
    Cliente Obter(int id);

}

