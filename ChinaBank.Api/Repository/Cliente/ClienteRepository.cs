using Dapper;
using System;
using System.Data.SQLite;
using ChinaBank.Api.Models;
using Microsoft.Extensions.Configuration;

namespace ChinaBank.Api.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private string _conn;

        public ClienteRepository(IConfiguration configuration)
        {
            _conn = DbConnection.getStringConnection(configuration);
        }

        public bool AbertaContaCliente(Cliente cliente, string numeroConta)
        {
            int result = 0;
            int clienteId = ProximoCliente();

            string sqlCliente = "insert into Cliente (ClienteId, ClienteDocumento, ClienteNome, ClienteEmail, ClienteCelular) " +
                                " values (@Id, @Documento, @Nome, @Email, @Celular)";

            string sqlConta = "insert into Conta (ContaNumero, ContaTipo, ContaAbertaEm, ContaSaldo, ClienteId) " +
                              " values (@Numero, @Tipo, @AbertaEm, @Saldo, @Id)";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.Execute(sqlCliente, new
                {
                    Id = clienteId,
                    Documento = cliente.Documento,
                    Nome = cliente.Nome,
                    Email = cliente.Email,
                    Celular = cliente.Celular
                });

                if (result > 0)
                {
                    result = connection.Execute(sqlConta, new
                    {
                        Numero = numeroConta,
                        Tipo = 0,
                        AbertaEm = System.DateTime.Now.ToString("yyyyMMddHHmmss"),
                        Saldo = 0,
                        Id = clienteId
                    });
                }
            }

            return (result == 0 ? false : true);
        }

        public bool Atualizar(Cliente cliente)
        {
            int result = 0;

            string sql = "update Cliente set ClienteEmail = @Emmail, ClienteCelular = @Celular" +
                         " where ClienteId = @Id";
                                            
            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.Execute(sql, new
                {
                    Id = cliente.Id,
                    Email = cliente.Email,
                    Celular = cliente.Celular
                });                
            }

            return (result == 0 ? false : true);
        }

        private int ProximoCliente()
        {
            Cliente result = null;
            string sql = "select max(ClienteId)+1 Id" +
                         "  from Cliente ";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.QueryFirstOrDefault<Cliente>(@sql);
            }

            return (result == null || result.Id == 0 ? 1 : result.Id);
        }

        public bool ClienteExiste(int id)
        {
            Cliente result = Obter(id);

            return (result == null ? false : true);
        }

        public bool ClienteExiste(string numeroDocumento)
        {
            Cliente result = null;
            string sql = "select ClienteId Id, ClienteNome Nome, ClienteDocumento Documento " +
                         "  from Cliente " +
                         " where Documento = @Documento";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.QueryFirstOrDefault<Cliente>(@sql, new
                {
                    Documento = numeroDocumento
                });
            }

            return (result == null ? false : true);
        }

        public Cliente Obter(int id)
        {
            Cliente result = null;
            string sql = "select ClienteId Id, ClienteDocumento Documento, ClienteNome Nome, ClienteEmail Email, ClienteCelular Celular" +
                         "  from Cliente " +
                         " where ClienteId = @Id";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.QueryFirstOrDefault<Cliente>(sql, new
                {
                    Id = id
                });
            }

            return result;
        }        
    }
}
