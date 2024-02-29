namespace ExtradosStore.Entities.DTOs.UserDTOs
{
    public class UserWithRolesNameDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string role { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public bool status { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime dateOfBirth { get; set; }
    }
}

