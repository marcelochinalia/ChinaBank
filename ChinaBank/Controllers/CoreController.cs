using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChinaBank.Api.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChinaBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoreController : ControllerBase
    {
        public CoreController()
        {

        }

        [HttpGet]

        public async Task<ActionResult> Login()
        {
            SegurancaHelper helper = new SegurancaHelper();
            var token = await helper.LoginAsync("0358934 - 3", "juju-chinalia@123");

            if (token == null)
            {
                Unauthorized();
            }
            else
            {
                Ok(token);
            }
        }
    }
}