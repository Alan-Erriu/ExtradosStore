﻿namespace ExtradosStore.Entities.Models
{
    public class Offer
    {
        public int offer_id { get; set; }
        public string offer_name { get; set; }
        public long offer_date_start { get; set; }
        public long offer_date_expiration { get; set; }
        public bool offer_status
        {
            get
            {
                return (offer_date_expiration > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) ? true : false;
            }
        }
        public int offer_userId { get; set; }

    }
}
