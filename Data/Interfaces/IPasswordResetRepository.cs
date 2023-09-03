using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Data.Interfaces
{
    public interface IPasswordResetRepository
    {
        Task<string> GenerateAndSaveCellphoneCodeForUserAsync(string email);
        Task<bool> ResetPasswordForUserAsync(string email, string newPassword);
        Task<bool> ValidateCodeForUserAsync(CodePassword codePassword);
        
        Task<string> GenerateAndSaveCellphoneCodeForAdminAsync(string email);
        Task<bool> ResetPasswordForAdminAsync(string email, string newPassword);
        Task<bool> ValidateCodeForAdminAsync(CodePassword codePassword);
    }
}