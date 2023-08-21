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
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationRepository _repository;

        public AuthController(IAuthenticationRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("LoginAdmin")]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginRequest request)
        {
            try 
            {
                var admin = await _repository.AuthenticateAdminAsync(request.Login, request.Password);
                var token = _repository.GenerateJwtToken(admin);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
        {
            try 
            {
                var user = await _repository.AuthenticateUserAsync(request.Login, request.Password);
                var token = _repository.GenerateJwtToken(user);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}