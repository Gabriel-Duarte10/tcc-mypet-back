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
        [HttpPost("InitiateResetUser")]
        public async Task<IActionResult> InitiateReset([FromBody] ResetPasswordInitiateRequest request)
        {
            try 
            {
                await _passwordRepository.GenerateAndSaveCellphoneCodeForUserAsync(request.Email);
                return Ok("Verification code sent to the registered cellphone.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CompleteResetUser")]
        public async Task<IActionResult> CompleteReset([FromBody] ResetPasswordCompleteRequest request)
        {
            try 
            {
                if (request.NewPassword != request.ConfirmNewPassword) return BadRequest("Passwords do not match.");
                await _passwordRepository.ValidateAndResetPasswordForUserAsync(request.Email, request.NewPassword, request.CellphoneCode);
                return Ok("Password successfully reset.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("InitiateResetAdmin")]
        public async Task<IActionResult> InitiateResetAdmin([FromBody] ResetPasswordInitiateRequest request)
        {
            try 
            {
                await _passwordRepository.GenerateAndSaveCellphoneCodeForAdminAsync(request.Email);
                return Ok("Verification code sent to the registered cellphone.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CompleteResetAdmin")]
        public async Task<IActionResult> CompleteResetAdmin([FromBody] ResetPasswordCompleteRequest request)
        {
            try 
            {
                if (request.NewPassword != request.ConfirmNewPassword) return BadRequest("Passwords do not match.");
                await _passwordRepository.ValidateAndResetPasswordForAdminAsync(request.Email, request.NewPassword, request.CellphoneCode);
                return Ok("Password successfully reset.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}