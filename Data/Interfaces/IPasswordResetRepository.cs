using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Interfaces
{
    public interface IPasswordResetRepository
    {
        Task GenerateAndSaveCellphoneCodeForUserAsync(string email);
        Task<bool> ValidateAndResetPasswordForUserAsync(string email, string newPassword, int cellphoneCode);
        
        Task GenerateAndSaveCellphoneCodeForAdminAsync(string email);
        Task<bool> ValidateAndResetPasswordForAdminAsync(string email, string newPassword, int cellphoneCode);
    }
}