using Microsoft.AspNetCore.Mvc;
using Security.Api.Helper;
using Security.Api.Models;
using System.Net;

namespace Security.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuardaController : ControllerBase
    {
        private IAcessoRepository _repo;
        

        public GuardaController(IAcessoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return BadRequest("Acesse negado.");            
        }

        [HttpPost]
        public ActionResult Post([FromBody] Acesso acesso)
        {
            string token = null;

            if (ModelState.IsValid)
            {
                GuardiaoHelper helper = new GuardiaoHelper(_repo);
                token = helper.ObterToken(acesso);
                if (token != null)
                {
                    return Ok(token);                    
                }
            }

            return BadRequest("Informações de acesso inválidas.");
        }

        [HttpPost]
        [Route("/api/guarda/create")]
        public ActionResult Create()
        {
            ContaCriada ret = null;

            if (ModelState.IsValid)
            {
                GuardiaoHelper helper = new GuardiaoHelper(_repo);
                ret = helper.CriarContaSegura();
                if (ret != null)
                {
                    return Ok(ret);
                }
                else
                {
                    return StatusCode(HttpHeaderSecurityVariables.HttpStatusCodeIntervalError);
                }
            }
            else
            {
                return BadRequest("Formato de Requisição Inválido");
            }                        
        }

        [HttpGet]
        [Route("/api/guarda/validate")]
        public ActionResult Validate()
        {
            string token =  Request.Headers[HttpHeaderSecurityVariables.Token_http_header].ToString();
            string numeroConta = Request.Headers[HttpHeaderSecurityVariables.Conta_http_header].ToString();

            if (token == null)
            {
                return Unauthorized();
            }

            TokenHelper helper = new TokenHelper(_repo);
            if ( helper.Autorizado(token, numeroConta) )
            {
                return Ok();
            }
            else
            {
                return Unauthorized();
            }            
        }
    }
}
