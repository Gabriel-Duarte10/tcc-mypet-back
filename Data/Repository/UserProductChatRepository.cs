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
    public class UserProductChatRepository : IUserProductChatRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserProductChatRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserProductChatSessionDTO> CreateSessionAsync(UserProductChatSessionRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var sessionDb = await _context.UserProductChatSessions.AddAsync(_mapper.Map<UserProductChatSession>(request));
                sessionDb.Entity.CreatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
                
                var session = await _context.UserProductChatSessions.Where(s => s.Id == sessionDb.Entity.Id)
                    .Include(s => s.User1)
                    .Include(s => s.User2)
                    .Include(s => s.Product)
                    .FirstOrDefaultAsync();

                await transaction.CommitAsync();

                return _mapper.Map<UserProductChatSessionDTO>(session);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error creating UserProductChatSession.", ex);
            }
        }


        public async Task<IEnumerable<UserProductChatSessionDTO>> ListSessionsByUserIdAsync(int userId)
        {
            var sessions = await _context.UserProductChatSessions
                .Include(s => s.User1)
                .Include(s => s.User2)
                .Include(s => s.Product)
                .Where(s => s.User1Id == userId || s.User2Id == userId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserProductChatSessionDTO>>(sessions);
        }

        public async Task<IEnumerable<UserProductChatDTO>> ListMessagesBySessionIdAsync(int sessionId)
        {
            var messages = await _context.UserProductChats
                .Include(x => x.SenderUser)
                .Where(m => m.UserProductChatSessionId == sessionId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserProductChatDTO>>(messages);
        }
    }
}