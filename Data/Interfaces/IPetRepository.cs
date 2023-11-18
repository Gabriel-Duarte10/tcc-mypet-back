using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Data.Interfaces
{
    public interface IPetRepository
    {
        Task<IEnumerable<PetDTO>> GetAllDashboardAsync();
        Task<IEnumerable<PetDTO>> GetAllAsync(int userId);
        Task<List<PetDTO>> GetFilteredPetsAsync(FilterModel filters);
        Task<PetDTO> GetByIdAsync(int id);
        Task<IEnumerable<PetDTO>> GetPetsByUserIdAsync(int userId);
        Task<PetDTO> CreateAsync(PetRequest request);
        Task<PetDTO> UpdateAsync(int id, PetRequest request);
        Task DeleteAsync(int id);
        Task<FavoritePetDto> AddToFavoriteAsync(FavoritePetRequest request);
        Task RemoveFromFavoritesAsync(int petId, int userId);
        Task<ReportedPetDto> ReportPetAsync(ReportedPetRequest request);
        Task UnreportPetAsync(int petId);
        Task<List<FavoritePetDto>> GetAllFavoritePetsAsync();
        Task<List<PetDTO>> GetFavoritePetsByUserIdAsync(int userId);
        Task<List<ReportedPetDto>> GetReportedPetsByUserIdAsync(int userId);
        Task<List<ReportedPetDto>> GetAllReportedPetsAsync();
    }
}
