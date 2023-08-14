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
    public class CharacteristicRepository : ICharacteristicRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CharacteristicRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CharacteristicDTO>> GetAllAsync()
        {
            var characteristics = await _context.Characteristics.ToListAsync();
            return _mapper.Map<IEnumerable<CharacteristicDTO>>(characteristics);
        }

        public async Task<CharacteristicDTO> GetByIdAsync(int id)
        {
            var characteristic = await _context.Characteristics.FindAsync(id);
            if (characteristic == null)
            {
                throw new Exception("Characteristic not found");
            }
            return _mapper.Map<CharacteristicDTO>(characteristic);
        }

        public async Task<CharacteristicDTO> CreateAsync(CharacteristicRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var characteristic = _mapper.Map<Characteristic>(request);
                await _context.Characteristics.AddAsync(characteristic);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<CharacteristicDTO>(characteristic);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error creating Characteristic.", ex);
            }
        }

        public async Task<CharacteristicDTO> UpdateAsync(int id, CharacteristicRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var characteristic = await _context.Characteristics.FindAsync(id);
                if (characteristic == null)
                {
                    throw new Exception("Characteristic not found");
                }
                _mapper.Map(request, characteristic);
                _context.Characteristics.Update(characteristic);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<CharacteristicDTO>(characteristic);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error updating Characteristic.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var characteristic = await _context.Characteristics.FindAsync(id);
                if (characteristic == null)
                {
                    throw new Exception("Characteristic not found");
                }
                _context.Characteristics.Remove(characteristic);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error deleting Characteristic.", ex);
            }
        }
    }
}
