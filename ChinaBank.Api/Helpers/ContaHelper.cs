using ChinaBank.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChinaBank.Api.Helpers
{
    public class ContaHelper
    {
        private readonly IContaRepository _repo;
        
        public ContaHelper(IContaRepository repo)
        {
            _repo = repo;          
        }

        public Saldo ObterSaldo(string token)
        {
            string numeroConta = SegurancaClienteHelper.DecodificarNumeroConta(token);
            return _repo.ObterSaldo(numeroConta);
        }

        public bool Depositar(Deposito deposito)
        {
            return _repo.Depositar(deposito);
        }

        public bool Transferir(Transferencia transferencia)
        {
            return _repo.Transferir(transferencia);
        }

        public List<string> validarDeposito(Deposito deposito)
        {
            List<string> lista = new List<string>();

            if (string.IsNullOrEmpty(deposito.NumeroConta) || string.IsNullOrWhiteSpace(deposito.NumeroConta))
            {
                lista.Add("Conta de Origem não informada.");
            }
            else
            {
                if (!_repo.ContaExiste(deposito.NumeroConta))
                {
                    lista.Add("Conta inválida.");
                }
            }

            if (deposito.ValorMonetario < 0)
            {
                lista.Add("Valor de depósito inválido.");
            }

            return lista;
        }

        public List<string> validarTransferencia(Transferencia transferencia)
        {
            List<string> lista = new List<string>();

            if (string.IsNullOrEmpty(transferencia.NumeroConta) || string.IsNullOrWhiteSpace(transferencia.NumeroConta))
            {
                lista.Add("Conta de Origem não informada.");
            }   
            else
            {
                if (!_repo.ContaExiste(transferencia.NumeroConta))
                {
                    lista.Add("Conta Origem inválida.");
                }
            }

            if (string.IsNullOrEmpty(transferencia.NumeroContaDestino) || string.IsNullOrWhiteSpace(transferencia.NumeroContaDestino))
            {
                lista.Add("Conta Destino não informada.");
            }
            else
            {
                if (!_repo.ContaExiste(transferencia.NumeroContaDestino))
                {
                    lista.Add("Conta Destino inválida.");
                }
            }

            if (transferencia.ValorMonetario <0)
            {
                lista.Add("Valor de transferência inválido.");
            }
            else
            {
                Saldo saldo = _repo.ObterSaldo(transferencia.NumeroConta);
                if (transferencia.ValorMonetario > saldo.SaldoAtual)
                {
                    lista.Add("Valor de transferência não permitido.");
                }
            }

            return lista;
        }

        public ExtratoPaginado ObterExtrato(string numeroConta, int ultimosDias, int pagina, int tamanhoPagina)
        {
            ExtratoFiltro filtro = new ExtratoFiltro(numeroConta, ultimosDias, pagina, tamanhoPagina);
            return _repo.ObterExtrato(filtro);
        }
    }
}
