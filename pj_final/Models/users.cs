using System.ComponentModel.DataAnnotations;

namespace pj_final.Models
{
    public class users
    {
        [Key] public int id_user {  get; set; }
        public string full_name { get; set; }
        public string email {  get; set; }
        public string password {  get; set; }
        public string confirmar_clave { get; set; }

        public bool restablecer { get; set; }
        public bool confirmado { get; set; }
        public string token { get; set; }
    }
}
