using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Data.Interfaces
{
    public interface IAdministratorRepository
    {
        Task<IEnumerable<AdministratorDto>> GetAllAsync();
        Task<AdministratorDto> GetByIdAsync(int id);
        Task<AdministratorDto> CreateAsync(AdministratorRequest request);
        Task<AdministratorDto> UpdateAsync(int id, AdministratorRequest request);
        Task DeleteAsync(int id);
    }
}