using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using tcc_mypet_back.Data.Context;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Models;
using BCrypt.Net;

namespace tcc_mypet_back.Data.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DataContext _context;
        private readonly JwtSettingsDTO _jwtSettings;

        public AuthenticationRepository(DataContext context, JwtSettingsDTO jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings;
        }

        public async Task<Administrator> AuthenticateAdminAsync(string login, string password)
        {
            try
            {
                var admin = await _context.Administrators.FirstOrDefaultAsync(a => a.Email == login);
                if (admin == null)
                    throw new Exception("Administrator not found.");

                if(!BCrypt.Net.BCrypt.Verify(password, admin.Password))
                    throw new Exception("Invalid password.");

                return admin;
            }
            catch (System.Exception error)
            {
                
                throw;
            }
        }

    public async Task<User> AuthenticateUserAsync(string login, string password)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login);
            if (user == null)
                throw new Exception("User not found.");

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new Exception("Invalid password.");

            return user;
        }
        catch (System.Exception error)
        {
            
            throw;
        }
    }
        public string GenerateJwtToken(Administrator admin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                    new Claim("name", admin.Name),
                    new Claim(ClaimTypes.Email, admin.Email),
                    new Claim(ClaimTypes.Role, "Administrator"),
                    new Claim("cellphone", admin.Cellphone)
                }),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.HoursExpire),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.MobilePhone, user.Cellphone),
                    new Claim(ClaimTypes.PostalCode, user.ZipCode),
                    new Claim(ClaimTypes.StreetAddress, user.Street),
                    new Claim(ClaimTypes.StateOrProvince, user.State),
                    new Claim(ClaimTypes.Locality, user.City),
                    new Claim("Number", user.Number.ToString()),
                    new Claim("Status", user.IsActive.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.HoursExpire),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}