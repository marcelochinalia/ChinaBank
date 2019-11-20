using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SQLite;
using Security.Api.Models;
using System.IO;
using System;

namespace Security.Api.Repositories
{
    public class SegurancaRepository : IAcessoRepository
    {
        protected string _chave = "t3st3-ch@v3-ch1naB@nk";
        private IConfiguration _conf;
        private string _conn;

        public SegurancaRepository(IConfiguration configuration)
        {
            var path = new DirectoryInfo(path: Directory.GetCurrentDirectory()).FullName;

            _conf = configuration;
            _conn = _conf.GetSection("ConnectionStrings:ChinaDbContext").Value;
            _conn = string.Format(_conn, path);
        }

        private Token CriarToken(string numeroConta)
        {
            Token t = null;

            long exp = long.Parse(System.DateTime.Now.AddMinutes(5).ToString("yyyyMMddHHmmss"));
            
            t = new Token
            {
                Chave = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(numeroConta + _chave + exp.ToString().Trim())),
                Expiracao = exp
            };

            return t;
        }

        private ContaCriada ExisteConta(Acesso acesso)
        {
            ContaCriada result = null;
            string sql = "SELECT ContaId Id, ContaNumero NumeroConta" +
                         "  FROM Conta " +
                         " WHERE ContaNumero = '" + acesso.NumeroConta + "'" +
                         "   AND ContaSenha  = '" + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(acesso.Senha)) + "'";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.QueryFirstOrDefault<ContaCriada>(sql);
            }

            return result;
        }   
        
        private int ProximoId()
        {
            int ret = 1;
            Chave result = null;
            string sql = "select max(contaid)+1 Id from conta";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.QueryFirstOrDefault<Chave>(@sql);
            }

            ret = (result == null ? 1 : (result.Id == 0 ? 1 : result.Id));

            return ret;
        }

        private string GravarToken(ContaCriada conta)
        {
            string ret = null;
            int result = 0;            
            Token token = CriarToken(conta.NumeroConta);
            
            string sql = "update conta " +
                         "   set ContaToken = '" + token.Chave + "', " +
                         "       ContaExpiracao = " + token.Expiracao.ToString() +
                         " where ContaId = " + conta.Id;

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.Execute(sql);
            }

            if (result > 0)
            {
                ret = token.Chave;
            }

            return ret;
        }

        public string ObterToken(Acesso acesso)
        {
            String token = null;
            ContaCriada c = ExisteConta(acesso);
            
            if (c != null)
            {
                token = GravarToken(c);
            }
            
            return token;            
        }

        private Acesso GerarNumeroConta()
        {
            Random numAleatorio = new Random();
            long conta = numAleatorio.Next(100001, Int32.MaxValue) * numAleatorio.Next(1,9);
            string numeroConta = conta.ToString().Substring(0,5) + "-" + conta.ToString().Substring(6,1);
             
            return new Acesso {
                NumeroConta = numeroConta,
                //pra efeito didático e facilitar o uso, todas as senhas serão 654321
                Senha = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("654321"))
            };
        }

        public ContaCriada CriarContaSegura()
        {
            Acesso acesso = GerarNumeroConta();
            ContaCriada ret = null;
            int id = ProximoId();
            int result = 0;
            
            string sql = "insert into conta (ContaId, ContaNumero, ContaSenha, ContaToken, ContaExpiracao) " +
                           "  values( " + id + ",'" + acesso.NumeroConta + "', '" + acesso.Senha + "', null, null)";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.Execute(sql);
            }

            if (result > 0)
            {
                ret = new ContaCriada
                {
                    Id = id,
                    NumeroConta = acesso.NumeroConta
                };
            }           

            return ret;            
        }

        public bool ValidarToken(string token, string numeroConta)
        {
            bool ret = false;
            Token result = null;
            string sql = "SELECT ContaNumero NumeroConta, ContaToken Chave, ContaExpiracao Expiracao " +
                         "  FROM Conta " +
                         " WHERE ContaToken = '" + token + "'";

            using (SQLiteConnection connection = new SQLiteConnection(_conn))
            {
                result = connection.QueryFirstOrDefault<Token>(sql);
            }

            if (result != null)
            {
                long agora = long.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmss"));
                
                if (result.NumeroConta.Equals(numeroConta) && agora <= result.Expiracao) 
                {
                    ret = true;
                }
            }

            return ret;
        }
    }
}
