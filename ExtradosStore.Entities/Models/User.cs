
namespace ExtradosStore.Entities.Models
{
    public class User
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_lastname { get; set; }
        public string user_email { get; set; }
        public string user_password_hash { get; set; }
        public int user_roleid { get; set; }
        public bool user_status { get; set; }
        public long user_created_at { get; set; }
        public long date_of_birth { get; set; }

    }
}
