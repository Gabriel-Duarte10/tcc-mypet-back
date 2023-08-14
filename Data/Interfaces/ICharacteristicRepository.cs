using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Data.Interfaces
{
    public interface ICharacteristicRepository
    {
        Task<IEnumerable<CharacteristicDTO>> GetAllAsync();
        Task<CharacteristicDTO> GetByIdAsync(int id);
        Task<CharacteristicDTO> CreateAsync(CharacteristicRequest request);
        Task<CharacteristicDTO> UpdateAsync(int id, CharacteristicRequest request);
        Task DeleteAsync(int id);
    }
}
