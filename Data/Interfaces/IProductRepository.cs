using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Data.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(int id);
        Task<ProductDTO> CreateAsync(ProductRequest request);
        Task<ProductDTO> UpdateAsync(int id, ProductRequest request);
        Task DeleteAsync(int id);
        Task<FavoriteProductDto> AddToFavoriteAsync(FavoriteProductRequest request);
        Task RemoveFromFavoritesAsync(int productId, int userId);
        Task<ReportedProductDto> ReportProductAsync(ReportedProductRequest request);
        Task UnreportProductAsync(int productId);
        Task<List<FavoriteProductDto>> GetAllFavoriteProductsAsync();
        Task<List<FavoriteProductDto>> GetFavoriteProductsByUserIdAsync(int userId);

    }
}
