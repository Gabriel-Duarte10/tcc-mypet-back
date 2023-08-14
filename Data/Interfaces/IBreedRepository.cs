using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Data.Interfaces
{
    public interface IBreedRepository
    {
        Task<IEnumerable<BreedDTO>> GetAllAsync();
        Task<BreedDTO> GetByIdAsync(int id);
        Task<BreedDTO> CreateAsync(BreedRequest request);
        Task<BreedDTO> UpdateAsync(int id, BreedRequest request);
        Task DeleteAsync(int id);
    }
}
