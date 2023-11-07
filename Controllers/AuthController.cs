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
        private readonly IPasswordResetRepository _passwordRepository;

        public AuthController(IAuthenticationRepository repository, IPasswordResetRepository passwordResetRepository)
        {
            _repository = repository;
            _passwordRepository = passwordResetRepository;
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
                return BadRequest(new { message = ex.Message });
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
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("InitiateResetUser")]
        public async Task<IActionResult> InitiateReset([FromBody] ResetPasswordInitiateRequest request)
        {
            try 
            {
                var cellphone = await _passwordRepository.GenerateAndSaveCellphoneCodeForUserAsync(request.Email);
                return Ok(cellphone);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("CompleteResetUser")]
        public async Task<IActionResult> CompleteReset([FromBody] ResetPasswordCompleteRequest request)
        {
            try 
            {
                if (request.NewPassword != request.ConfirmNewPassword) return BadRequest("Passwords do not match.");
                
                return Ok(await _passwordRepository.ResetPasswordForUserAsync(request.Email, request.NewPassword));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("InitiateResetAdmin")]
        public async Task<IActionResult> InitiateResetAdmin([FromBody] ResetPasswordInitiateRequest request)
        {
            try 
            {
                var cellphone = await _passwordRepository.GenerateAndSaveCellphoneCodeForAdminAsync(request.Email);
                return Ok(cellphone);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("CompleteResetAdmin")]
        public async Task<IActionResult> CompleteResetAdmin([FromBody] ResetPasswordCompleteRequest request)
        {
            try 
            {
                if (request.NewPassword != request.ConfirmNewPassword) return BadRequest("Passwords do not match.");
                await _passwordRepository.ResetPasswordForAdminAsync(request.Email, request.NewPassword);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("ValidateCodeUser")]
        public async Task<IActionResult> ValidateCodeUser([FromBody] CodePassword request)
        {
            try 
            {
                
                return Ok(await _passwordRepository.ValidateCodeForUserAsync(request));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("ValidateCodeAdmin")]
        public async Task<IActionResult> ValidateCodeAdmin([FromBody] CodePassword request)
        {
            try 
            {
                await _passwordRepository.ValidateCodeForAdminAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}