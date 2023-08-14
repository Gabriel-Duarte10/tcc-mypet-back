using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Repositories;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacteristicsController : ControllerBase
    {
        private readonly ICharacteristicRepository _repository;

        public CharacteristicsController(ICharacteristicRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var characteristics = await _repository.GetAllAsync();
                return Ok(characteristics);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var characteristic = await _repository.GetByIdAsync(id);
                if (characteristic == null)
                {
                    return NotFound(new { message = "Characteristic not found" });
                }
                return Ok(characteristic);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CharacteristicRequest request)
        {
            try
            {
                var characteristic = await _repository.CreateAsync(request);
                return Ok(characteristic);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CharacteristicRequest request)
        {
            try
            {
                var characteristic = await _repository.UpdateAsync(id, request);
                return Ok(characteristic);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
