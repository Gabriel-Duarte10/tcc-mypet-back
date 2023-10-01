using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using tcc_mypet_back.Data.Context;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Models;
using tcc_mypet_back.Data.Request;
using tcc_mypet_back.Extensions;

namespace tcc_mypet_back.Data.Repository
{
    public class PetRepository : IPetRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PetRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PetDTO>> GetAllAsync()
        {
            var pets = await _context.Pets
            .Include(x => x.Breed)
            .Include(x => x.User).ToListAsync();
            var petDtos = _mapper.Map<IEnumerable<PetDTO>>(pets);

            var petImages = await _context.PetImages.ToListAsync();
            var petImageDtos = _mapper.Map<List<PetImageDTO>>(petImages);

            foreach (var pet in petDtos)
            {
                pet.PetImages = petImageDtos.Where(pi => pi.PetId == pet.Id).ToList();
            }

            return petDtos;
        }
        public async Task<List<PetDTO>> GetFilteredPetsAsync(FilterModel filters)
        {
            var query = await _context.Pets
            .Include(x => x.User)
            .Include(x => x.Breed).ToListAsync();

            if (filters.AnimalTypeId.HasValue)
                query = query.Where(p => p.Breed.AnimalTypeId == filters.AnimalTypeId).ToList();

            if (filters.BreedId.HasValue)
                query = query.Where(p => p.BreedId == filters.BreedId).ToList();

            if (filters.CharacteristicId.HasValue)
                query = query.Where(p => p.CharacteristicId == filters.CharacteristicId).ToList();

            if (filters.SizeId.HasValue)
                query = query.Where(p => p.SizeId == filters.SizeId).ToList();

            if (filters.Status.HasValue)
                query = query.Where(p => p.AdoptionStatus == filters.Status).ToList();

            if (filters.MinAge.HasValue)
            {
                var minBirthYear = DateTime.Now.Year - filters.MinAge.Value;
                query = query.Where(p => p.BirthYear <= minBirthYear).ToList();
            }

            if (filters.MaxAge.HasValue)
            {
                var maxBirthYear = DateTime.Now.Year - filters.MaxAge.Value;
                query = query.Where(p => p.BirthYear >= maxBirthYear).ToList();
            }

            if (filters.StartDate.HasValue)
                query = query.Where(p => p.CreatedAt >= filters.StartDate.Value).ToList();

            if (filters.EndDate.HasValue)
                query = query.Where(p => p.CreatedAt <= filters.EndDate.Value).ToList();

            if(filters.State != null)
                query = query.Where(p => p.User.State == filters.State).ToList();

            var petDtos = _mapper.Map<List<PetDTO>>(query);

            return petDtos;
        }
        public async Task<PetDTO> GetByIdAsync(int id)
        {
            var pet = await _context.Pets
            .Include(x => x.Breed)
            .Include(x => x.User).FirstOrDefaultAsync(p => p.Id == id);
            if (pet == null) throw new Exception("Pet not found.");

            var petImages = await _context.PetImages.Where(pi => pi.PetId == pet.Id).ToListAsync();
            var petDto = _mapper.Map<PetDTO>(pet);
            petDto.PetImages = _mapper.Map<List<PetImageDTO>>(petImages);

            return petDto;
        }
        // Dentro da classe PetRepository

        public async Task<IEnumerable<PetDTO>> GetPetsByUserIdAsync(int userId)
        {
            var pets = await _context.Pets
            .Include(x => x.Breed)
            .Include(x => x.User).Where(p => p.UserId == userId).ToListAsync();
            var petDtos = _mapper.Map<IEnumerable<PetDTO>>(pets);

            var petImages = await _context.PetImages.ToListAsync();
            var petImageDtos = _mapper.Map<List<PetImageDTO>>(petImages);

            foreach (var pet in petDtos)
            {
                pet.PetImages = petImageDtos.Where(pi => pi.PetId == pet.Id).ToList();
            }

            return petDtos;
        }

        public async Task<PetDTO> CreateAsync(PetRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var pet = _mapper.Map<Pet>(request);
                pet.CreatedAt = DateTime.UtcNow;
                var petDb = await _context.Pets.AddAsync(pet);

                await _context.SaveChangesAsync();

                if (request.Images != null && request.Images.Count > 6)
                    throw new Exception("Cannot attach more than 6 images.");

                foreach (var file in request.Images)
                {
                    var base64Image = ImageExtensions.ConvertFileToBase64(file);
                    var petImage = new PetImage
                    {
                        ImageName = file.FileName,
                        Image64 = base64Image,
                        PetId = petDb.Entity.Id
                    };
                    _context.PetImages.Add(petImage);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetByIdAsync(pet.Id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error creating pet.", ex);
            }
        }

        public async Task<PetDTO> UpdateAsync(int id, PetRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id);
                if (pet == null) throw new Exception("Pet not found.");

                _mapper.Map(request, pet);

                if (request.Images != null && request.Images.Count > 6)
                    throw new Exception("Cannot attach more than 6 images.");

                pet.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var existingImages = await _context.PetImages.Where(pi => pi.PetId == pet.Id).ToListAsync();
                _context.PetImages.RemoveRange(existingImages); // Remove existing images

                await _context.SaveChangesAsync();

                foreach (var file in request.Images)
                {
                    var base64Image = ImageExtensions.ConvertFileToBase64(file);
                    var petImage = new PetImage
                    {
                        ImageName = file.FileName,
                        Image64 = base64Image,
                        PetId = pet.Id
                    };
                    _context.PetImages.Add(petImage);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return await GetByIdAsync(pet.Id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error updating pet.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id);
                if (pet == null) throw new Exception("Pet not found.");

                var existingImages = await _context.PetImages.Where(pi => pi.PetId == pet.Id).ToListAsync();
                _context.PetImages.RemoveRange(existingImages);

                pet.DeleteAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error deleting pet.", ex);
            }
        }

        public async Task<FavoritePetDto> AddToFavoriteAsync(FavoritePetRequest request)
        {
            var favorite = await _context.FavoritePets
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(f => f.PetId == request.PetId && f.UserId == request.UserId);
            if (favorite != null)
            {
                favorite.DeleteAt = null;
                favorite.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                favorite = _mapper.Map<FavoritePet>(request);
                favorite.CreatedAt = DateTime.UtcNow;
                await _context.FavoritePets.AddAsync(favorite);
            }
            await _context.SaveChangesAsync();

            return _mapper.Map<FavoritePetDto>(favorite);
        }

        public async Task RemoveFromFavoritesAsync(int petId, int userId)
        {
            var favorite = await _context.FavoritePets
                            .FirstOrDefaultAsync(f => f.PetId == petId && f.UserId == userId);

            if (favorite == null)
                throw new Exception("Favorite pet not found.");

            favorite.DeleteAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task<ReportedPetDto> ReportPetAsync(ReportedPetRequest request)
        {
            var reported = await _context.ReportedPets
            .FirstOrDefaultAsync(r => r.PetId == request.PetId);

            if (reported != null)
            {
                reported.Counter += 1;
            }
            else
            {
                reported = new ReportedPet { PetId = request.PetId, Counter = 1 };
                reported.CreatedAt = DateTime.UtcNow;
                await _context.ReportedPets.AddAsync(reported);
            }

            await _context.SaveChangesAsync();

            return _mapper.Map<ReportedPetDto>(reported);
        }

        public async Task UnreportPetAsync(int petId)
        {
            var reported = await _context.ReportedPets.FirstOrDefaultAsync(r => r.PetId == petId);

            if (reported == null)
                throw new Exception("Reported pet not found.");

            reported.Counter -= 1;
            if (reported.Counter == 0)
            {
                _context.ReportedPets.Remove(reported);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<FavoritePetDto>> GetAllFavoritePetsAsync()
        {
            var favorites = await _context.FavoritePets.ToListAsync();
            return _mapper.Map<List<FavoritePetDto>>(favorites);
        }

        public async Task<List<FavoritePetDto>> GetFavoritePetsByUserIdAsync(int userId)
        {
            var favorites = await _context.FavoritePets
                            .Where(f => f.UserId == userId)
                            .ToListAsync();

            return _mapper.Map<List<FavoritePetDto>>(favorites);
        }
        public async Task<List<ReportedPetDto>> GetReportedPetsByUserIdAsync(int userId)
        {
            var reportedPets = await _context.ReportedPets
                .Include(x => x.Pet)
                .Where(rp => rp.Pet.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<ReportedPetDto>>(reportedPets);
        }

        public async Task<List<ReportedPetDto>> GetAllReportedPetsAsync()
        {
            var reportedPets = await _context.ReportedPets.ToListAsync();
            return _mapper.Map<List<ReportedPetDto>>(reportedPets);
        }

    }
}