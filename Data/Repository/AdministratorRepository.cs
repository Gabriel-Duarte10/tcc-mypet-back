using AutoMapper;
using Microsoft.EntityFrameworkCore;
using tcc_mypet_back.Data.Context;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Models;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Data.Repository
{
    public class AdministratorRepository : IAdministratorRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AdministratorRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AdministratorDto>> GetAllAsync()
        {
            var administrators = await _context.Set<Administrator>().ToListAsync();
            return _mapper.Map<IEnumerable<AdministratorDto>>(administrators);
        }

        public async Task<AdministratorDto> GetByIdAsync(int id)
        {
            var administrator = await _context.Set<Administrator>().FindAsync(id);
            if (administrator == null) throw new Exception("Administrator not found.");
            return _mapper.Map<AdministratorDto>(administrator);
        }

        public async Task<AdministratorDto> CreateAsync(AdministratorRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if(await _context.Set<Administrator>().AnyAsync(a => a.Email == request.Email))
                    throw new Exception("Email already in use.");

                var administrator = _mapper.Map<Administrator>(request);
                administrator.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                administrator.CreatedAt = DateTime.UtcNow;
                _context.Set<Administrator>().Add(administrator);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<AdministratorDto>(administrator);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<AdministratorDto> UpdateAsync(int id, AdministratorRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var administrator = await _context.Set<Administrator>().FindAsync(id);
                if (administrator == null) throw new Exception("Administrator not found.");

                _mapper.Map(request, administrator);
                administrator.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return _mapper.Map<AdministratorDto>(administrator);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ;
            }
        }

        public async Task DeleteAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var administrator = await _context.Set<Administrator>().FindAsync(id);
                if (administrator == null) throw new Exception("Administrator not found.");

                administrator.DeleteAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
