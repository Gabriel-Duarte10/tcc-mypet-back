using Microsoft.AspNetCore.Mvc;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace tcc_mypet_back.Controllers
{
    //[[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            return Ok(await _userRepository.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            try
            {
                return Ok(await _userRepository.GetByIdAsync(id));
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromForm] UserCreateRequest request)
        {
            try
            {
                var createdUser = await _userRepository.CreateAsync(request);
                return Ok(createdUser);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromForm] UserUpdateRequest request)
        {
            try
            {
                var updateUser = await _userRepository.UpdateAsync(id, request);
                return Ok(updateUser);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _userRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
