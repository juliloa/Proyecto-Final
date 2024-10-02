using System.ComponentModel.DataAnnotations;

namespace pj_final.Models
{
    public class users
    {
        [Key] public int id_user {  get; set; }
        public string full_name { get; set; }
        public string email {  get; set; }
        public string password {  get; set; }
    }
}
