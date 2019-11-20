using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System;

namespace ChinaBank.Api.Helper
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
            return JsonConvert.SerializeObject(this);
  
        }
    }

    public class SegurancaHelper
    {
        private readonly string WebApiGuardianBaseURL = "https://localhost:44310";

        public async Task<string> LoginAsync(string conta, string senha)
        {
            string ret = null;
            var json = new Acesso(conta, senha).toJson();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new System.Uri(WebApiGuardianBaseURL);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("Content-type", "application/json; charset=utf-8");
                client.DefaultRequestHeaders.Add("User-Agent", "ChinaBank.Api");

                var response = await client.PostAsync("/api/guardiao", content);

                var message = response.EnsureSuccessStatusCode();
                {
                    ret = message.Content.ReadAsStringAsync().Result;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return ret;
        }
    }
}
