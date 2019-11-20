using System;
using System.Threading.Tasks;
using ChinaBank.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ChinaBank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcessoController : ControllerBase
    {
        private readonly int ServiceUnavailable = 503;

        public AcessoController()
        {

        }

        [HttpGet]
        public IActionResult Get()
        {
            return BadRequest("Acesso Negado ao ChinaBank.");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Acesso acesso) { 

            if (ModelState.IsValid)
            {
                try
                {
                    SegurancaClienteHelper helper = new SegurancaClienteHelper();
                    string token = await helper.Login(acesso);
                    if (token != null)
                    {
                        return Ok(token);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
                catch(Exception)
                {
                    return StatusCode(ServiceUnavailable);
                }
            }
            else
            {
                return BadRequest("Formato de requisição inválido.");
            }
        }
    }
}