using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetRepository _petRepository;

        public PetsController(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var pets = await _petRepository.GetAllAsync();
                return Ok(pets);
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
                var pet = await _petRepository.GetByIdAsync(id);
                return Ok(pet);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPetsByUserIdAsync(int userId)
        {
            try
            {
                var pets = await _petRepository.GetPetsByUserIdAsync(userId);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] PetRequest request)
        {
            try
            {
                var pet = await _petRepository.CreateAsync(request);
                return Ok(pet);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] PetRequest request)
        {
            try
            {
                var pet = await _petRepository.UpdateAsync(id, request);
                return Ok(pet);
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
                await _petRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("favorite")]
        public async Task<IActionResult> AddToFavoriteAsync([FromBody] FavoritePetRequest request)
        {
            try
            {
                var favorite = await _petRepository.AddToFavoriteAsync(request);
                return Ok(favorite);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("favorite")]
        public async Task<IActionResult> RemoveFromFavoritesAsync([FromQuery] int petId, [FromQuery] int userId)
        {
            try
            {
                await _petRepository.RemoveFromFavoritesAsync(petId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("report")]
        public async Task<IActionResult> ReportPetAsync([FromBody] ReportedPetRequest request)
        {
            try
            {
                var report = await _petRepository.ReportPetAsync(request);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("report")]
        public async Task<IActionResult> UnreportPetAsync([FromQuery] int petId)
        {
            try
            {
                await _petRepository.UnreportPetAsync(petId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> GetAllFavoritePetsAsync()
        {
            try
            {
                var favorites = await _petRepository.GetAllFavoritePetsAsync();
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("favorites/{userId}")]
        public async Task<IActionResult> GetFavoritePetsByUserIdAsync(int userId)
        {
            try
            {
                var favorites = await _petRepository.GetFavoritePetsByUserIdAsync(userId);
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("reported/user/{userId}")]
        public async Task<IActionResult> GetReportedPetsByUserIdAsync(int userId)
        {
            try
            {
                var reportedPets = await _petRepository.GetReportedPetsByUserIdAsync(userId);
                return Ok(reportedPets);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("reported")]
        public async Task<IActionResult> GetAllReportedPetsAsync()
        {
            try
            {
                var reportedPets = await _petRepository.GetAllReportedPetsAsync();
                return Ok(reportedPets);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}