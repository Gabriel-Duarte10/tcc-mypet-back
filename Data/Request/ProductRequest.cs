namespace tcc_mypet_back.Data.Request
{
    public class ProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
    public class FavoriteProductRequest
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
    }
    public class ReportedProductRequest
    {
        public int ProductId { get; set; }
    }
}
