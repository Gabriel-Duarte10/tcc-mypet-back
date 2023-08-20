using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Data.Interfaces
{
    public interface IUserProductChatRepository
    {
        Task<UserProductChatSessionDTO> CreateSessionAsync(UserProductChatSessionRequest request);
        Task<IEnumerable<UserProductChatSessionDTO>> ListSessionsByUserIdAsync(int userId);
        Task<IEnumerable<UserProductChatDTO>> ListMessagesBySessionIdAsync(int sessionId);
    }
}