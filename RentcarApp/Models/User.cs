
namespace RentcarApp.Models
{
    public class User
    {
        public int id { get; set; }
        public string? name_surname { get; set; }
        public int authority_id { get; set; }
        public string? password { get; set; }
        public string? e_mail { get; set; }
    }
}
