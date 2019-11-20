using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ChinaBank.Api.Helpers
{
    public class Acesso
    {
        public Acesso(string numeroConta, string senha)
        {
            NumeroConta = numeroConta;
            Senha = senha;
        }

        public string NumeroConta { get; set; }
        public string Senha { get; set; }

        public string toJson()
        {
            return JsonSerializer.Serialize(this);

        }
    }

    public class ContaCriada
    {
        public int Id { get; set; }
        public string NumeroConta { get; set; }

        public static ContaCriada FromJson(string obj)
        {
            return JsonSerializer.Deserialize<ContaCriada>(obj);
        }
    }

    public class SegurancaClienteHelper
    {
        private HttpClient _client;
        public string Token { get; set; }
        public string NumeroConta { get; set; }

        public SegurancaClienteHelper()
        {
            _client = new HttpClient();
            _client.BaseAddress = new System.Uri(Rotas.BaseURL);
            _client.DefaultRequestHeaders.Add("User-Agent", "ChinaBank.Api");
        }

        public async Task<string> Login(Acesso acesso)
        {
            string ret = null;
            var json = acesso.toJson();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(Rotas.ObterToken, content);
                if (response.IsSuccessStatusCode)
                {
                    ret = response.Content.ReadAsStringAsync().Result;
                    return ret;
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case (System.Net.HttpStatusCode.Unauthorized):
                            return ret;
                        case (System.Net.HttpStatusCode.BadRequest):
                            return ret;
                        default:
                            throw new HttpRequestException();
                    }
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<bool> Autorizado(HttpRequest request) {
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri(Rotas.BaseURL);
            client.DefaultRequestHeaders.Add("User-Agent", "ChinaBank.Api");

            Token = request.Headers[Rotas.Token_http_header].ToString();
            NumeroConta = request.Headers[Rotas.Conta_http_header].ToString();
            
            if (Token == null || NumeroConta == null)
            {
                return false;
            }

            try
            {
                client.DefaultRequestHeaders.Add(Rotas.Token_http_header, Token);
                client.DefaultRequestHeaders.Add(Rotas.Conta_http_header, NumeroConta);
                var response = await client.GetAsync(Rotas.ValidarToken);

                switch (response.StatusCode)
                {
                    case (System.Net.HttpStatusCode.Unauthorized):
                        return false;
                    case (System.Net.HttpStatusCode.OK):
                        return true;
                    default:
                        throw new HttpRequestException();
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ContaCriada> CriarContaSegura()
        {
            ContaCriada ret = null;
            var json = "";
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _client.PostAsync(Rotas.CriarConta, content);
                if (response.IsSuccessStatusCode)
                {
                    string aux = response.Content.ReadAsStringAsync().Result;
                    return ContaCriada.FromJson(aux);
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case (System.Net.HttpStatusCode.Unauthorized):
                            return ret;
                        case (System.Net.HttpStatusCode.BadRequest):
                            return ret;
                        default:
                            throw new HttpRequestException();
                    }
                }
            }
            catch (HttpRequestException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string DecodificarNumeroConta(string token)
        {
            return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(token)).Substring(0, 7);
        }
    }
}
