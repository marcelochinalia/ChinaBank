using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Security.Api.Models
{
    public class Token
    {
        public string NumeroConta { get; set; }
        public string Chave { get; set; }
        public long Expiracao { get; set; }
    }
}
