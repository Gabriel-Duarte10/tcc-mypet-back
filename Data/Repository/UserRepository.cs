﻿using AutoMapper;
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
using tcc_mypet_back.Services;

namespace tcc_mypet_back.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration configuration;
        private readonly ImageService _imagesService;
        private string ApiKeyGoogle = "";

        public UserRepository(DataContext context, IMapper mapper, IConfiguration configuration, ImageService imagesService)
        {
            _context = context;
            _mapper = mapper;
            this.configuration = configuration;
            ApiKeyGoogle = this.configuration["Google:KeyGeocoding"] ?? "";
            _imagesService = imagesService;
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
                if(await _context.Users.AnyAsync(u => u.Email == request.Email))
                    throw new Exception("Email already in use.");
                    
                var user = _mapper.Map<User>(request);
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // Obtendo a latitude e longitude usando a classe auxiliar
                string fullAddress = $"{request.Street}, {request.Number}, {request.City}, {request.State}, {request.ZipCode}";
                var geocodingHelper = new GeocodingHelper(configuration["GoogleMaps:KeyGeocoding"] ?? ""); // Supondo que sua chave da API está em appsettings sob "GoogleMaps:ApiKey"
                var (latitude, longitude) = await geocodingHelper.GetLatLongFromAddress(fullAddress);
                user.Latitude = latitude;
                user.Longitude = longitude;

                if (request.Images != null && request.Images.Count > 3)
                    throw new Exception("Cannot attach more than 3 images.");

                user.CreatedAt = DateTime.UtcNow;
                var userDb = await _context.Users.AddAsync(user);

                await _context.SaveChangesAsync();
                if(request.Images != null)
                {
                    var newImages = await _imagesService.UploadImageFireBase(request.Images);
                    foreach (var image in newImages)
                    {
                        var userImage = new UserImage()
                        {
                            ImageName = image.NameImage,
                            Image64 = image.UrlImage,
                            UserId = userDb.Entity.Id,
                            CreatedAt = DateTime.Now
                        };
                        await _context.UserImages.AddAsync(userImage);
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
                // Obtendo a latitude e longitude usando a classe auxiliar
                string fullAddress = $"{request.Street}, {request.Number}, {request.City}, {request.State}, {request.ZipCode}";
                var geocodingHelper = new GeocodingHelper(configuration["GoogleMaps:KeyGeocoding"] ?? ""); // Supondo que sua chave da API está em appsettings sob "GoogleMaps:ApiKey"
                var (latitude, longitude) = await geocodingHelper.GetLatLongFromAddress(fullAddress);
                user.Latitude = latitude;
                user.Longitude = longitude;
                
                await _context.SaveChangesAsync();

                var existingImages = await _context.UserImages.Where(ui => ui.UserId == user.Id).ToListAsync();
                if(existingImages.Count > 0)
                {
                    _context.UserImages.RemoveRange(existingImages); // Remove existing images
                    _imagesService.DeleteImagesFireBase(existingImages.Select(x => x.ImageName).ToList());
                }

                await _context.SaveChangesAsync();
                var newImages = await _imagesService.UploadImageFireBase(request.Images);
                foreach (var image in newImages)
                {
                    var userImage = new UserImage()
                    {
                        ImageName = image.NameImage,
                        Image64 = image.UrlImage,
                        UserId = user.Id,
                        CreatedAt = DateTime.Now
                    };
                    await _context.UserImages.AddAsync(userImage);
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
