using Admin.Api.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Admin.Api.Helpers
{

    public class AdminHelper
    {
        private HttpClient _client;

        public ContaPaginado _ContaPaginado;
        public ExtratoPaginado _ExtratoPaginado;
        public Cliente _Cliente;

        public AdminHelper()
        {
            _client = new HttpClient();
            _client.BaseAddress = new System.Uri(Rotas._BaseUrlChinaBankApi);
            _client.DefaultRequestHeaders.Add("User-Agent", "Admin.Api");
            _client.DefaultRequestHeaders.Add("usuario", "admin");
            _client.DefaultRequestHeaders.Add("senha", "admin");
        }

        public async Task<HttpStatusCode> AcionarListagemContas(int pagina, int tamanhoPagina)
        {
            try
            {
                string url = Rotas._ListagemContas.
                        Replace("{0}", pagina.ToString().Trim()).
                                Replace("{1}", tamanhoPagina.ToString().Trim());

                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string aux = response.Content.ReadAsStringAsync().Result;
                    _ContaPaginado = ContaPaginado.FromJson(aux);                    
                }
                return response.StatusCode;
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

        public async Task<HttpStatusCode> AcionarListagemMovimentacoes(string numeroConta, int pagina, int tamanhoPagina)
        {
            try
            {
                string url = Rotas._ListagemMovimentacoesConta.
                        Replace("{0}", numeroConta.Trim()).
                            Replace("{1}", pagina.ToString().Trim()).
                                Replace("{2}", tamanhoPagina.ToString().Trim());

                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string aux = response.Content.ReadAsStringAsync().Result;
                    _ExtratoPaginado = ExtratoPaginado.FromJson(aux);
                }
                return response.StatusCode;
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

        public async Task<HttpStatusCode> AcionarDetalhesCliente(int id)
        {
            try
            {
                string url = Rotas._DetalhesCliente.Replace("{0}", id.ToString().Trim());

                var response = await _client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string aux = response.Content.ReadAsStringAsync().Result;
                    _Cliente = Cliente.FromJson(aux);
                }
                return response.StatusCode;
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
    }
}
