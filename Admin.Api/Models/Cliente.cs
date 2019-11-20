using Newtonsoft.Json;

namespace Admin.Api.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }

        public static Cliente FromJson(string obj)
        {
            return JsonConvert.DeserializeObject<Cliente>(obj);
        }
    }
}
