using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Data.Interfaces
{
    public interface IUserPetChatRepository
    {
        Task<UserPetChatSessionDTO> CreateSessionAsync(UserPetChatSessionRequest request);
        Task<IEnumerable<UserPetChatSessionDTO>> ListSessionsByUserIdAsync(int userId);
        Task<IEnumerable<UserPetChatDTO>> ListMessagesBySessionIdAsync(int sessionId);
    }
}