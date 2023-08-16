namespace tcc_mypet_back.Data.Dtos
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public List<ProductImageDTO> ProductImages { get; set; }
    }

    public class ProductImageDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageName { get; set; }
        public string Image64 { get; set; }
    }
}
