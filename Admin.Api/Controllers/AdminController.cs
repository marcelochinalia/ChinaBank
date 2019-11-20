using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Admin.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private AdminHelper _helper;

        public AdminController()
        {
            _helper = new AdminHelper();
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return BadRequest("Acesso Negado.");
        }

        [HttpGet]
        [Route("/api/[controller]/contas")]
        public async Task<IActionResult> ObterContasAsync([FromQuery] int pagina, int tamanhoPagina)
        {
            HttpStatusCode ret = await _helper.AcionarListagemContas(pagina, tamanhoPagina);
            switch (ret)
            { 
                case HttpStatusCode.OK:            
                    return Ok(_helper._ContaPaginado);
                
                case HttpStatusCode.NotFound:
                    return NotFound();

                case HttpStatusCode.BadRequest:
                    return BadRequest();

                default:
                    return StatusCode(int.Parse(ret.ToString()));
            }
        }

        [HttpGet]
        [Route("/api/[controller]/{numeroConta}/movimentacoes")]
        public async Task<IActionResult> ObterMovimentacoesAsync(string numeroConta, [FromQuery] int pagina, int tamanhoPagina)
        {
            HttpStatusCode ret = await _helper.AcionarListagemMovimentacoes(numeroConta, pagina, tamanhoPagina);
            switch (ret)
            {
                case HttpStatusCode.OK:
                    return Ok(_helper._ExtratoPaginado);

                case HttpStatusCode.NotFound:
                    return NotFound();

                case HttpStatusCode.BadRequest:
                    return BadRequest();

                default:
                    return StatusCode(int.Parse(ret.ToString()));
            }
        }

        [HttpGet]
        [Route("/api/[controller]/cliente/{id}")]
        public async Task<IActionResult> ObterDetalhesClienteAsync(int id)
        {
            HttpStatusCode ret = await _helper.AcionarDetalhesCliente(id);
            switch (ret)
            {
                case HttpStatusCode.OK:
                    return Ok(_helper._Cliente);

                case HttpStatusCode.NotFound:
                    return NotFound();

                case HttpStatusCode.BadRequest:
                    return BadRequest();

                default:
                    return StatusCode(int.Parse(ret.ToString()));
            }
        }
    }
}
