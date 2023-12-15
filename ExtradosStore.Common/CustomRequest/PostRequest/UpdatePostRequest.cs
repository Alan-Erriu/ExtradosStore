namespace ExtradosStore.Common.CustomRequest.PostRequest
{
    public class UpdatePostRequest
    {
        public int postId { get; set; }
        public string? postName { get; set; }
        public string? postDescription { get; set; }
        public decimal? postPrice { get; set; }
        public int? postStock { get; set; }
        public int? postCategoryId { get; set; }
        public int? postBrandId { get; set; }
    }
}
