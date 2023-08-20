using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPetChatController : ControllerBase
    {
        private readonly IUserPetChatRepository _repository;

        public UserPetChatController(IUserPetChatRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("CreateSession")]
        public async Task<IActionResult> CreateSession([FromBody] UserPetChatSessionRequest request)
        {
            try
            {
                var session = await _repository.CreateSessionAsync(request);
                return Ok(session);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ListSessions/{userId}")]
        public async Task<IActionResult> ListSessionsByUserId(int userId)
        {
            try
            {
                var sessions = await _repository.ListSessionsByUserIdAsync(userId);
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ListMessages/{sessionId}")]
        public async Task<IActionResult> ListMessagesBySessionId(int sessionId)
        {
            try
            {
                var messages = await _repository.ListMessagesBySessionIdAsync(sessionId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}