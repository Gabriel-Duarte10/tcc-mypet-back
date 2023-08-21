using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Controllers
{
    [Authorize(Roles = "Administrator")]
    [ApiController]
    [Route("api/[controller]")]
    public class SizesController : ControllerBase
    {
        private readonly ISizeRepository _sizeRepository;

        public SizesController(ISizeRepository sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var sizes = await _sizeRepository.GetAllAsync();
                return Ok(sizes);
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
                var size = await _sizeRepository.GetByIdAsync(id);
                return Ok(size);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] SizeRequest request)
        {
            try
            {
                var size = await _sizeRepository.CreateAsync(request);
                return Ok(size);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] SizeRequest request)
        {
            try
            {
                var size = await _sizeRepository.UpdateAsync(id, request);
                return Ok(size);
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
                await _sizeRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
