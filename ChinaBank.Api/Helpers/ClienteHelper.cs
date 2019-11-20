using ChinaBank.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChinaBank.Api.Helpers
{
    public class ClienteHelper
    {
        private readonly IClienteRepository _repo;
        
        public ClienteHelper(IClienteRepository repo)
        {
            _repo = repo;
        }

        public List<string> CadastroValidado(Cliente cliente)
        {
            List<string> ret = new List<string>();

            if (string.IsNullOrEmpty(cliente.Nome) || string.IsNullOrWhiteSpace(cliente.Nome))
            {
                ret.Add("Precisamos do seu nome completo.");
            }

            if (string.IsNullOrEmpty(cliente.Documento) || string.IsNullOrWhiteSpace(cliente.Documento))
            {
                ret.Add("Precisamos do seu CPF.");
            }
            else {
                if (cliente.Id == 0 && DocumentoJaExiste(cliente.Documento))
                {
                    ret.Add("CPF informado já existe em nossa base!");
                }
            }

            if (string.IsNullOrEmpty(cliente.Email) || string.IsNullOrWhiteSpace(cliente.Email))
            { 
                ret.Add("Informe seu e-mail.");
            }

            if (string.IsNullOrEmpty(cliente.Celular) || string.IsNullOrWhiteSpace(cliente.Celular))
            { 
                ret.Add("Informe seu celular.");
            }

            return ret;
        }

        public async Task<string> Novo(Cliente cliente)
        {
            string ret = null;
            try
            {
                SegurancaClienteHelper helper = new SegurancaClienteHelper();
                ContaCriada conta = await helper.CriarContaSegura();

                if (conta != null)
                {
                    if (_repo.AbertaContaCliente(cliente, conta.NumeroConta))
                    {
                        ret = conta.NumeroConta;
                    }
                }
            }
            catch (Exception)
            {
                ret = null;
            }
            
            return ret;
        }

        public bool Atualizar(Cliente cliente)
        {
            return _repo.Atualizar(cliente);
                   
        }

        public bool Existe(int id)
        {
            return _repo.ClienteExiste(id);
        }

        public bool DocumentoJaExiste(string documento)
        {
            return _repo.ClienteExiste(documento);
        }

        public Cliente Obter(int id)
        {
            return _repo.Obter(id);
        }
    }
}
