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

namespace tcc_mypet_back.Data.Repository
{
    public class UserPetChatRepository : IUserPetChatRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserPetChatRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserPetChatSessionDTO> CreateSessionAsync(UserPetChatSessionRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var sessionDb =  await _context.UserPetChatSessions.AddAsync(_mapper.Map<UserPetChatSession>(request));
                
                sessionDb.Entity.CreatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                var session = await _context.UserPetChatSessions.Where(s => s.Id == sessionDb.Entity.Id)
                    .Include(s => s.User1)
                    .Include(s => s.User2)
                    .Include(s => s.Pet)
                    .FirstOrDefaultAsync();

                await transaction.CommitAsync();
                return _mapper.Map<UserPetChatSessionDTO>(session);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error creating UserPetChatSession.", ex);
            }
        }

        public async Task<IEnumerable<UserPetChatSessionDTO>> ListSessionsByUserIdAsync(int userId)
        {
            var sessions = await _context.UserPetChatSessions
                .Include(s => s.User1)
                .Include(s => s.User2)
                .Include(s => s.Pet)
                .Where(s => s.User1Id == userId || s.User2Id == userId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserPetChatSessionDTO>>(sessions);
        }

        public async Task<IEnumerable<UserPetChatDTO>> ListMessagesBySessionIdAsync(int sessionId)
        {
            var messages = await _context.UserPetChats
                .Include(x => x.SenderUser)
                .Where(m => m.UserPetChatSessionId == sessionId)
                .ToListAsync();
                try
                {
                    var mappedData = _mapper.Map<IEnumerable<UserPetChatDTO>>(messages);
                }
                catch (AutoMapperMappingException ex)
                {
                    // Aqui, ex é a exceção completa que pode fornecer detalhes sobre qual propriedade causou o erro.
                    throw;
                }

            return _mapper.Map<IEnumerable<UserPetChatDTO>>(messages);
        }
    }
}