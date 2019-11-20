using System.Collections.Generic;
using System.Threading.Tasks;
using ChinaBank.Api.Helpers;
using ChinaBank.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChinaBank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private ClienteHelper _helper;

        public ClienteController(IClienteRepository repo)
        {
            _helper = new ClienteHelper(repo);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return BadRequest("Rota Inválida.");
        }

        [HttpGet]
        [Route("/api/[controller]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!await new SegurancaClienteHelper().Autorizado(Request)) { return Unauthorized(); }

            Cliente c =_helper.Obter(id);
            if (c == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(c);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Formato de requisição inválido.");
            }

            List<string> alertas = _helper.CadastroValidado(cliente);
            if (alertas.Count > 0)
            {
                return BadRequest(alertas);
            }

            string numeroConta = await _helper.Novo(cliente);
            if (numeroConta != null)
            {
                return Ok("Parabéns, conta " + numeroConta + " aberta. Você é o novo cliente ChinaBank! :))");
            }
            else
            {
                return StatusCode(Rotas.HttpStatusCodeIntervalError, "Desculpe! :o( Não conseguimos processar sua transação.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Cliente cliente)
        {
            if (!await new SegurancaClienteHelper().Autorizado(Request)) { return Unauthorized();  }

            if (!ModelState.IsValid) { return BadRequest("Formato de requisição inválido."); }

            if (!_helper.Existe(cliente.Id))
            {
                return NotFound();
            }

            List<string> alertas = _helper.CadastroValidado(cliente);
            if (alertas.Count > 0)
            {
                return BadRequest(alertas);
            }

            if (_helper.Atualizar(cliente))
            {
                return Ok("Dados atualizados com sucesso! ;o)");
            }
            else
            {
                return StatusCode(Rotas.HttpStatusCodeIntervalError, "Desculpe! :o( Não conseguimos processar sua transação.");
            }
        }
    }
}
