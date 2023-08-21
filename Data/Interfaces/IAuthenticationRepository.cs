using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tcc_mypet_back.Data.Models;

namespace tcc_mypet_back.Data.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<Administrator> AuthenticateAdminAsync(string login, string password);
        Task<User> AuthenticateUserAsync(string login, string password);
        string GenerateJwtToken(Administrator admin);
        string GenerateJwtToken(User user);
    }
}