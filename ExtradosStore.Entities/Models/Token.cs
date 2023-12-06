namespace ExtradosStore.Entities.Models
{
    public class Token
    {
        public int token_id { get; set; }
        public int token_iduser { get; set; }
        public string token_accesstoken { get; set; }
        public string token_refreshToken { get; set; }
        public long token_expiration_date_refreshtoken { get; set; }
    }
}
