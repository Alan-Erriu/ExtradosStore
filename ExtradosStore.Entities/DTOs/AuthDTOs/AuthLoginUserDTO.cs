

namespace ExtradosStore.Entities.DTOs.AuthDTOs
{
    public class AuthLoginUserDTO
    {


        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_email { get; set; }
        public string user_password_hash { get; set; }
        public int user_roleid { get; set; }
        public bool user_status { get; set; }
        public DateTime user_date_of_birth { get; set; }

    }
}
