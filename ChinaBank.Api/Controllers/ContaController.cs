using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChinaBank.Api.Helpers;
using ChinaBank.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChinaBank.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaController : ControllerBase
    {
        private ContaHelper _helper;

        public ContaController(IContaRepository repo)
        {
            _helper = new ContaHelper(repo);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return BadRequest("Rota Inválida.");
        }

        [HttpGet]
        [Route("/api/[controller]/saldo")]
        public async Task<IActionResult> ObterSaldo()
        {
            SegurancaClienteHelper sec = new SegurancaClienteHelper();
            if (!await sec.Autorizado(Request)) { return Unauthorized(); }

            Saldo saldo = null;
            try
            {
                
                saldo = _helper.ObterSaldo(sec.Token);

                if (saldo != null)
                {
                    return Ok(saldo);
                }
                else
                {
                    return NotFound("Conta não encontrada!");
                }
            }
            catch (Exception)
            {
                return StatusCode(Rotas.HttpStatusCodeIntervalError, "Desculpe! :o( Não conseguimos processar sua transação.");
            }
        }

        [HttpGet]
        [Route("/api/[controller]/extrato")]
        public async Task<IActionResult> ObterExtrato([FromQuery] int ultimosDias, int pagina, int tamanhoPagina)
        {
            SegurancaClienteHelper sec = new SegurancaClienteHelper();
            if (!await sec.Autorizado(Request)) { return Unauthorized(); }

            ExtratoPaginado paginado = null;
            try
            {
                paginado = _helper.ObterExtrato(sec.NumeroConta, ultimosDias, pagina, tamanhoPagina);

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
                return StatusCode(Rotas.HttpStatusCodeIntervalError, "Desculpe! :o( Não conseguimos processar sua transação.");
            }
        }

        [HttpPost]
        [Route("/api/[controller]/deposito")]
        public async Task<IActionResult> Depositar(Deposito deposito)
        {
            if (!await new SegurancaClienteHelper().Autorizado(Request)) { return Unauthorized(); }

            if (!ModelState.IsValid)
            {
                return BadRequest("Formato de requisição inválido.");
            }

            bool depositado = false;
            try
            {
                List<string> validacoes = _helper.validarDeposito(deposito);
               
                if (validacoes.Count > 0)
                {
                    return BadRequest(validacoes);
                }

                depositado = _helper.Depositar(deposito);
                if (depositado)
                {
                    return Ok("Deposito realizado. ;o)");
                }
                else
                {
                    return StatusCode(Rotas.HttpStatusCodeIntervalError, "Desculpe! :o( Não conseguimos processar sua transação.");
                }
            }
            catch (Exception)
            {
                return StatusCode(Rotas.HttpStatusCodeIntervalError, "Desculpe! :o( Não conseguimos processar sua transação.");
            }
        }

        [HttpPost]
        [Route("/api/[controller]/transferencia")]
        public async Task<IActionResult> Transferir(Transferencia transferencia)
        {
            if (!await new SegurancaClienteHelper().Autorizado(Request)) { return Unauthorized(); }

            if (!ModelState.IsValid)
            {
                return BadRequest("Formato de requisição inválido.");
            }

            try
            {
                List<string> validacoes = _helper.validarTransferencia(transferencia);

                if (validacoes.Count > 0)
                {
                    return BadRequest(validacoes);
                }

                bool transferido = _helper.Transferir(transferencia);
                if (transferido)
                {
                    return Ok("Transferência realizada. ;o)");
                }
                else
                {
                    return StatusCode(Rotas.HttpStatusCodeIntervalError, "Desculpe! :o( Não conseguimos processar sua transação.");
                }
            }
            catch (Exception)
            {
                return StatusCode(Rotas.HttpStatusCodeIntervalError, "Desculpe! :o( Não conseguimos processar sua transação.");
            }
        }
    }
}