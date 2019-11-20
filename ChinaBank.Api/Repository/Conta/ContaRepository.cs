using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SQLite;
using ChinaBank.Api.Models;
using System.Collections.Generic;

namespace ChinaBank.Api.Repository
{
    public class ContaRepository : IContaRepository
    {
        private readonly string _conn;

        public ContaRepository(IConfiguration configuration)
        {
            _conn = DbConnection.getStringConnection(configuration);
        }
        
        private int ProximoExtrato()
        {
            Extrato result = null;
            string sql = "select max(ExtratoId)+1 Id" +
                         "  from Extrato ";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.QueryFirstOrDefault<Extrato>(@sql);
            }

            return (result == null || result.Id == 0 ? 1 : result.Id);
        }

        public bool ContaExiste(string numeroConta)
        {
            Saldo result = null;
            string sql = "select ContaNumero NumeroConta, ContaSaldo SaldoAtual " +
                         "  from Conta " +
                         " where ContaNumero = @NumeroConta";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.QueryFirstOrDefault<Saldo>(@sql, new
                {
                    NumeroConta = numeroConta
                });
            }

            return (result == null ? false : true);
        }

        public bool Depositar(Deposito deposito)
        {
            int result = 0;
            int extratoId = ProximoExtrato();

            string sqlExtrato = "insert into Extrato (ExtratoId, ContaNumero, ExtratoDataOperacao, ExtratoValorOperacao, ExtratoTipoOperacao, ExtratoTransacao) " +
                                " values (@Id, @Numero, @DataOperacao, @ValorMonetario, @TipoOperacao, @Transacao)";

            string sqlConta = "update Conta set ContaSaldo = ContaSaldo + @ValorMonetario " +
                              " Where ContaNumero = @Numero";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.Execute(sqlExtrato, new
                {
                    Id = extratoId,
                    Numero = deposito.NumeroConta,
                    DataOperacao = long.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss")),
                    ValorMonetario = deposito.ValorMonetario,
                    TipoOperacao = "C",
                    Transacao = "Depósito"
                });

                if (result > 0)
                {
                    result = connection.Execute(sqlConta, new
                    {
                        ValorMonetario = deposito.ValorMonetario,
                        Numero = deposito.NumeroConta
                    });
                }
            }

            return (result > 0 ? true : false);
        }

        public Saldo ObterSaldo(string numeroConta)
        {
            Saldo result = null;
            string sql = "select ContaNumero NumeroConta, ContaSaldo SaldoAtual " +
                         "  from Conta " +
                         " where ContaNumero = @NumeroConta";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.QueryFirstOrDefault<Saldo>(@sql, new
                {
                    NumeroConta = numeroConta
                });
            }

            return result;
        }

        public ExtratoPaginado ObterExtrato(ExtratoFiltro filtro)
        {
            ExtratoPaginado ret = new ExtratoPaginado(filtro);
            List<Extrato> result = new List<Extrato>();

            string sql = "select ExtratoId Id, ContaNumero NumeroConta, ExtratoDataOperacao DataOperacao, " +
                         "       ExtratoValorOperacao ValorOperacao, ExtratoTipoOperacao TipoOperacao, ExtratoTransacao Transacao" +
                         "  from Extrato " +
                         " where ContaNumero = @NumeroConta " +
                         "   and ExtratoDataOperacao between @Passado and @Agora" +
                         " limit @TamanhoPagina offset @RegistroPontoPartida";
           
            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                string vNumeroConta = filtro.NumeroConta;
                long vPassado = long.Parse(System.DateTime.Now.AddDays(filtro.UltimosDias *- 1).ToString("yyyyMMdd") + "000000");
                long vAgora = long.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
                int vRegistroPontoPartida = (filtro.TamanhoPagina * filtro.Pagina) - filtro.TamanhoPagina;
                int vTamanhoPagina = filtro.TamanhoPagina + 1;

                result = connection.Query<Extrato>(@sql, new
                {
                    NumeroConta = vNumeroConta,
                    Passado = vPassado,
                    Agora = vAgora,
                    RegistroPontoPartida = vRegistroPontoPartida,
                    TamanhoPagina = vTamanhoPagina
                }).AsList<Extrato>();
            }

            if (result != null)
            {
                ret.ProximaPagina = "0";
                ret.PaginaAnterior = (filtro.Pagina > 1 ? (filtro.Pagina - 1).ToString() : "0");

                if (result.Count > filtro.TamanhoPagina)
                {
                    ret.ProximaPagina = (filtro.Pagina + 1).ToString();
                    result.RemoveAt(result.Count - 1);                    
                }
                
                ret.Extratos.AddRange(result);
            }
            
            return ret;
        }

        public bool Transferir(Transferencia transferencia)
        {
            int result = 0;
            int extratoIdDe = ProximoExtrato();
            int extratoIdPara = extratoIdDe + 1;

            string sqlExtratoDe = "insert into Extrato (ExtratoId, ContaNumero, ExtratoDataOperacao, ExtratoValorOperacao, ExtratoTipoOperacao, ExtratoTransacao) " +
                                  " values (@Id, @Numero, @DataOperacao, @ValorMonetario, @TipoOperacao, @Transacao)";

            string sqlExtratoPara = "insert into Extrato (ExtratoId, ContaNumero, ExtratoDataOperacao, ExtratoValorOperacao, ExtratoTipoOperacao, ExtratoTransacao) " +
                                    " values (@Id, @Numero, @DataOperacao, @ValorMonetario, @TipoOperacao, @Transacao)";

            string sqlContaDe = "update Conta set ContaSaldo = ContaSaldo - @ValorMonetario " +
                                " Where ContaNumero = @Numero";

            string sqlContaPara = "update Conta set ContaSaldo = ContaSaldo + @ValorMonetario " +
                                  " Where ContaNumero = @Numero";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.Execute(sqlExtratoDe, new
                {
                    Id = extratoIdDe,
                    Numero = transferencia.NumeroConta,
                    DataOperacao = long.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss")),
                    ValorMonetario = transferencia.ValorMonetario,
                    TipoOperacao = "D",
                    Transacao = "Transf Conta: " + transferencia.NumeroContaDestino
                });

                if (result > 0)
                {
                    result = connection.Execute(sqlContaDe, new
                    {
                        ValorMonetario = transferencia.ValorMonetario,
                        Numero = transferencia.NumeroConta
                    });
                }

                if (result > 0)
                {
                    result = connection.Execute(sqlExtratoPara, new
                    {
                        Id = extratoIdPara,
                        Numero = transferencia.NumeroContaDestino,
                        DataOperacao = long.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss")),
                        ValorMonetario = transferencia.ValorMonetario,
                        TipoOperacao = "C",
                        Transacao = "Receb. Transf Conta: " + transferencia.NumeroConta
                    });
                }

                if (result > 0)
                {
                    result = connection.Execute(sqlContaPara, new
                    {
                        ValorMonetario = transferencia.ValorMonetario,
                        Numero = transferencia.NumeroContaDestino
                    });
                }

                return (result > 0 ? true : false);
            }
        }        
    }
}
