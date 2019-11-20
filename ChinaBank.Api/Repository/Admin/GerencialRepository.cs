using ChinaBank.Api.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace ChinaBank.Api.Repository
{
    public class GerencialRepository : IGerencialRepository
    {
        private string _conn;
        private IConfiguration _conf;

        public GerencialRepository(IConfiguration configuration)
        {
            _conn = DbConnection.getStringConnection(configuration);
            _conf = configuration;
        }

        public ContaPaginado ObterContas(ContaFiltro filtro)
        {
            ContaPaginado ret = new ContaPaginado(filtro);
            List<Conta> result = new List<Conta>();

            string sql = "select ContaNumero NumeroConta, " +
                         "       ContaSaldo SaldoAtual, " +
                         "       '/api/admin/cliente/' || ClienteId Cliente, " +
                         "       '/api/admin/' || ContaNumero || '/movimentacoes?pagina=1&tamanhopagina=25' Extrato" +
                         "  from Conta " +
                         " order by ContaNumero " +
                         " limit @TamanhoPagina offset @RegistroPontoPartida";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                int vRegistroPontoPartida = (filtro.TamanhoPagina * filtro.Pagina) - filtro.TamanhoPagina;
                int vTamanhoPagina = filtro.TamanhoPagina + 1;

                result = connection.Query<Conta>(@sql, new
                {
                    RegistroPontoPartida = vRegistroPontoPartida,
                    TamanhoPagina = vTamanhoPagina
                }).AsList<Conta>();
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

                ret.Contas.AddRange(result);
            }

            return ret;
        }

        public ExtratoPaginado ObterMovimentacoes(ExtratoFiltro filtro)
        {
            ContaRepository cRepo = new ContaRepository(_conf);
            return cRepo.ObterExtrato(filtro);
        }
    }
}
