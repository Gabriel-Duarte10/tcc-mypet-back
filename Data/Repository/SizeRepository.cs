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
    public class SizeRepository : ISizeRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SizeRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SizeDTO>> GetAllAsync()
        {
            var sizes = await _context.Sizes.ToListAsync();
            return _mapper.Map<IEnumerable<SizeDTO>>(sizes);
        }

        public async Task<SizeDTO> GetByIdAsync(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                throw new Exception("Size not found");
            }
            return _mapper.Map<SizeDTO>(size);
        }

        public async Task<SizeDTO> CreateAsync(SizeRequest request)
        {
            var size = _mapper.Map<Size>(request);
            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();
            return _mapper.Map<SizeDTO>(size);
        }

        public async Task<SizeDTO> UpdateAsync(int id, SizeRequest request)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                throw new Exception("Size not found");
            }
            _mapper.Map(request, size);
            _context.Sizes.Update(size);
            await _context.SaveChangesAsync();
            return _mapper.Map<SizeDTO>(size);
        }

        public async Task DeleteAsync(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                throw new Exception("Size not found");
            }
            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();
        }
    }
}
