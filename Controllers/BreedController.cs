using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class BreedsController : ControllerBase
    {
        private readonly IBreedRepository _breedRepository;

        public BreedsController(IBreedRepository breedRepository)
        {
            _breedRepository = breedRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var breeds = await _breedRepository.GetAllAsync();
                return Ok(breeds);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var breed = await _breedRepository.GetByIdAsync(id);
                if (breed == null) return NotFound("Breed not found.");
                return Ok(breed);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] BreedRequest request)
        {
            try
            {
                var breed = await _breedRepository.CreateAsync(request);
                return Ok(breed);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] BreedRequest request)
        {
            try
            {
                var breed = await _breedRepository.UpdateAsync(id, request);
                if (breed == null) return NotFound("Breed not found.");
                return Ok(breed);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _breedRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
