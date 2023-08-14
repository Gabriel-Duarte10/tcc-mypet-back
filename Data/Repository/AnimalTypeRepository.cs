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
    public class AnimalTypeRepository : IAnimalTypeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AnimalTypeRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AnimalTypeDTO>> GetAllAsync()
        {
            var animalTypes = await _context.AnimalTypes.ToListAsync();
            return _mapper.Map<IEnumerable<AnimalTypeDTO>>(animalTypes);
        }

        public async Task<AnimalTypeDTO> GetByIdAsync(int id)
        {
            var animalType = await _context.AnimalTypes.FindAsync(id);
            if (animalType == null)
            {
                throw new Exception("AnimalType not found");
            }
            return _mapper.Map<AnimalTypeDTO>(animalType);
        }

        public async Task<AnimalTypeDTO> CreateAsync(AnimalTypeRequest request)
        {
            var animalType = _mapper.Map<AnimalType>(request);
            await _context.AnimalTypes.AddAsync(animalType);
            await _context.SaveChangesAsync();
            return _mapper.Map<AnimalTypeDTO>(animalType);
        }

        public async Task<AnimalTypeDTO> UpdateAsync(int id, AnimalTypeRequest request)
        {
            var animalType = await _context.AnimalTypes.FindAsync(id);
            if (animalType == null)
            {
                throw new Exception("AnimalType not found");
            }
            _mapper.Map(request, animalType);
            _context.AnimalTypes.Update(animalType);
            await _context.SaveChangesAsync();
            return _mapper.Map<AnimalTypeDTO>(animalType);
        }

        public async Task DeleteAsync(int id)
        {
            var animalType = await _context.AnimalTypes.FindAsync(id);
            if (animalType == null)
            {
                throw new Exception("AnimalType not found");
            }
            _context.AnimalTypes.Remove(animalType);
            await _context.SaveChangesAsync();
        }
    }
}
