using AutoMapper;
using Microsoft.EntityFrameworkCore;
using tcc_mypet_back.Data.Context;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Models;
using tcc_mypet_back.Data.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tcc_mypet_back.Extensions;

namespace tcc_mypet_back.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

            var userImages = await _context.UserImages.ToListAsync();
            var userImageDtos = _mapper.Map<List<UserImageDto>>(userImages);

            foreach (var user in userDtos)
            {
                user.UserImages = userImageDtos.Where(ui => ui.UserId == user.Id).ToList();
            }

            return userDtos;
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) throw new Exception("User not found.");
            var userImages = await _context.UserImages.Where(ui => ui.UserId == user.Id).ToListAsync();
            var userDto = _mapper.Map<UserDto>(user);
            userDto.UserImages = _mapper.Map<List<UserImageDto>>(userImages);

            return userDto;
        }

        public async Task<UserDto> CreateAsync(UserCreateRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = _mapper.Map<User>(request);
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

                if (request.Images != null && request.Images.Count > 3)
                    throw new Exception("Cannot attach more than 3 images.");

                user.CreatedAt = DateTime.UtcNow;
                var userDb = await _context.Users.AddAsync(user);

                await _context.SaveChangesAsync();
                if(request.Images != null)
                {
                    foreach (var file in request.Images)
                    {
                        var base64Image = ImageExtensions.ConvertFileToBase64(file);
                        var userImage = new UserImage
                        {
                            ImageName = file.FileName,
                            Image64 = base64Image,
                            UserId = userDb.Entity.Id
                        };
                        _context.UserImages.Add(userImage);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetByIdAsync(user.Id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error creating user.", ex);
            }
        }

        public async Task<UserDto> UpdateAsync(int id, UserUpdateRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null) throw new Exception("User not found.");

                _mapper.Map(request, user);

                // Handle image updates
                if (request.Images.Count > 3)
                    throw new Exception("Cannot attach more than 3 images.");

                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var existingImages = await _context.UserImages.Where(ui => ui.UserId == user.Id).ToListAsync();
                _context.UserImages.RemoveRange(existingImages); // Remove existing images

                await _context.SaveChangesAsync();

                foreach (var file in request.Images)
                {
                    var base64Image = ImageExtensions.ConvertFileToBase64(file);
                    var userImage = new UserImage
                    {
                        ImageName = file.FileName,
                        Image64 = base64Image,
                        UserId = user.Id
                    };
                    _context.UserImages.Add(userImage);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetByIdAsync(user.Id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error updating user.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null) throw new Exception("User not found.");

                var existingImages = await _context.UserImages.Where(ui => ui.UserId == user.Id).ToListAsync();
                _context.UserImages.RemoveRange(existingImages); // Remove existing images
               
                user.DeleteAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error deleting user.", ex);
            }
        }
    }
}
