using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using tcc_mypet_back.Data.Context;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Models;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Data.Repositories
{
    public class BreedRepository : IBreedRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BreedRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BreedDTO>> GetAllAsync()
        {
            var breeds = await _context.Breeds.ToListAsync();
            return _mapper.Map<IEnumerable<BreedDTO>>(breeds);
        }

        public async Task<BreedDTO> GetByIdAsync(int id)
        {
            var breed = await _context.Breeds.FindAsync(id);
            if (breed == null)
            {
                throw new Exception("Breed not found");
            }
            return _mapper.Map<BreedDTO>(breed);
        }

        public async Task<BreedDTO> CreateAsync(BreedRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var breed = _mapper.Map<Breed>(request);
                await _context.Breeds.AddAsync(breed);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<BreedDTO>(breed);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error creating Breed.", ex);
            }
        }

        public async Task<BreedDTO> UpdateAsync(int id, BreedRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var breed = await _context.Breeds.FindAsync(id);
                if (breed == null)
                {
                    throw new Exception("Breed not found");
                }
                _mapper.Map(request, breed);
                _context.Breeds.Update(breed);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<BreedDTO>(breed);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error updating Breed.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var breed = await _context.Breeds.FindAsync(id);
                if (breed == null)
                {
                    throw new Exception("Breed not found");
                }
                _context.Breeds.Remove(breed);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error deleting Breed.", ex);
            }
        }
    }
}
