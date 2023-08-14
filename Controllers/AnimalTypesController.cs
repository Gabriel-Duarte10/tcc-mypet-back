using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Repositories;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalTypesController : ControllerBase
    {
        private readonly IAnimalTypeRepository _repository;

        public AnimalTypesController(IAnimalTypeRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var animalTypes = await _repository.GetAllAsync();
                return Ok(animalTypes);
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
                var animalType = await _repository.GetByIdAsync(id);
                if (animalType == null)
                {
                    return NotFound(new { message = "AnimalType not found" });
                }
                return Ok(animalType);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(AnimalTypeRequest request)
        {
            try
            {
                var animalType = await _repository.CreateAsync(request);
                return Ok(animalType);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AnimalTypeRequest request)
        {
            try
            {
                var animalType = await _repository.UpdateAsync(id, request);
                return Ok(animalType);
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
