namespace ExtradosStore.Entities.Models
{
    public class Offer
    {
        public int offer_id { get; set; }
        public string offer_name { get; set; }
        public DateTime offer_date_start { get; set; }
        public DateTime offer_date_expiration { get; set; }
        public bool offer_status
        {
            get
            {
                return ((DateTimeOffset)offer_date_expiration).ToUnixTimeMilliseconds() > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            }
        }

        public int offer_userId { get; set; }

    }
}
