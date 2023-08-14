using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Data.Interfaces
{
    public interface ISizeRepository
    {
        Task<IEnumerable<SizeDTO>> GetAllAsync();
        Task<SizeDTO> GetByIdAsync(int id);
        Task<SizeDTO> CreateAsync(SizeRequest request);
        Task<SizeDTO> UpdateAsync(int id, SizeRequest request);
        Task DeleteAsync(int id);
    }
}
