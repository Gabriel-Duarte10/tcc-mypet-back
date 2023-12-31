using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tcc_mypet_back.Data.Context;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Request;
using tcc_mypet_back.Services;

namespace tcc_mypet_back.Data.Repository
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly DataContext _context;
        private readonly SMSService _smsService;

        public PasswordResetRepository(DataContext context, SMSService smsService)
        {
            _context = context;
            _smsService = smsService;
        }

        public async Task<string> GenerateAndSaveCellphoneCodeForUserAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) throw new Exception("User not found.");

            var code = new Random().Next(100000, 999999);
            user.CellphoneCode = code;
            await _context.SaveChangesAsync();

            await _smsService.SendSMSAsync(user.Cellphone, $"Your verification code is: {code}");

            return user.Cellphone;
        }

        public async Task<bool> ResetPasswordForUserAsync(string email, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) throw new Exception("User not found.");

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword); // Using BCrypt to hash the new password.
            user.CellphoneCode = null; // Reset the code.
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<string> GenerateAndSaveCellphoneCodeForAdminAsync(string email)
        {
            var admin = await _context.Administrators.FirstOrDefaultAsync(a => a.Email == email);
            if (admin == null) throw new Exception("Administrator not found.");

            var code = new Random().Next(100000, 999999);
            admin.CellphoneCode = code;
            await _context.SaveChangesAsync();

            await _smsService.SendSMSAsync(admin.Cellphone, $"Your verification code is: {code}");

            return admin.Cellphone;
        }

        public async Task<bool> ResetPasswordForAdminAsync(string email, string newPassword)
        {
            var admin = await _context.Administrators.FirstOrDefaultAsync(a => a.Email == email);
            if (admin == null) throw new Exception("Administrator not found.");

            admin.Password = BCrypt.Net.BCrypt.HashPassword(newPassword); // Using BCrypt to hash the new password.
            admin.CellphoneCode = null; // Reset the code.
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ValidateCodeForUserAsync(CodePassword codePassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == codePassword.Email);
            if (user == null) throw new Exception("User not found.");
            if(user.CellphoneCode != codePassword.CellphoneCode) throw new Exception("Invalid code.");

            return true;
        }

        public async Task<bool> ValidateCodeForAdminAsync(CodePassword codePassword)
        {
            var admin = await _context.Administrators.FirstOrDefaultAsync(a => a.Email == codePassword.Email);
            if (admin == null) throw new Exception("Administrator not found.");
            if(admin.CellphoneCode != codePassword.CellphoneCode) throw new Exception("Invalid code.");

            return true;
        }
    }
}