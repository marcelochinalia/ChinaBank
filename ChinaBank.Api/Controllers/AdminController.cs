using System;
using System.Threading.Tasks;
using ChinaBank.Api.Helpers;
using ChinaBank.Api.Helpers.Admin;
using ChinaBank.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChinaBank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private GerencialHelper _helper;
        private ClienteHelper _chelper;
        
        public AdminController(IGerencialRepository repo, IClienteRepository cRepo)
        {
            _helper = new GerencialHelper(repo);
            _chelper = new ClienteHelper(cRepo);            
        }

        public IActionResult Get()
        {
            return BadRequest("Acesso negado.");
        }

        [HttpGet]
        [Route("/api/[controller]/contas")]
        public IActionResult ObterContas([FromQuery] int pagina, int tamanhoPagina)
        {
            if (!SegurancaGerencialHelper.Autorizado(Request)) { return Unauthorized(); }

            ContaPaginado ret = null;
            try
            {
                ret = _helper.ObterContas(pagina, tamanhoPagina);
                if (ret.Contas.Count > 0)
                {
                    return Ok(ret);
                }
                else
                {
                    return NotFound(ret);
                }
            }
            catch (Exception)
            {
                return StatusCode(Rotas.HttpStatusCodeIntervalError);
            }
        }

        [HttpGet]
        [Route("/api/[controller]/{numeroConta}/movimentacoes")]
        public IActionResult ObterMovimentacao(string numeroConta, [FromQuery] int pagina, int tamanhoPagina)
        {
            if (!SegurancaGerencialHelper.Autorizado(Request)) { return Unauthorized(); }

            ExtratoPaginado paginado = null;
            try
            {
                paginado = _helper.ObterMovimentacoes(numeroConta, pagina, tamanhoPagina);

                if (paginado.Extratos.Count > 0)
                {
                    return Ok(paginado);
                }
                else
                {
                    return NotFound(paginado);
                }
            }
            catch (Exception)
            {
                return StatusCode(Rotas.HttpStatusCodeIntervalError);
            }
        }

        [HttpGet]
        [Route("/api/[controller]/cliente/{id}")]
        public IActionResult Get(int id)
        {
            if (!SegurancaGerencialHelper.Autorizado(Request)) { return Unauthorized(); }

            Cliente c = _chelper.Obter(id);
            if (c == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(c);
            }
        }
    }
}