using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Data.Interfaces
{
    public interface IAnimalTypeRepository
    {
        Task<IEnumerable<AnimalTypeDTO>> GetAllAsync();
        Task<AnimalTypeDTO> GetByIdAsync(int id);
        Task<AnimalTypeDTO> CreateAsync(AnimalTypeRequest request);
        Task<AnimalTypeDTO> UpdateAsync(int id, AnimalTypeRequest request);
        Task DeleteAsync(int id);
    }
}
