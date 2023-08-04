using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Controllers
{
    [Route("api/administrator")]
    [ApiController]
    public class AdministratorsController : ControllerBase
    {
        private readonly IAdministratorRepository _administratorRepository;

        public AdministratorsController(IAdministratorRepository administratorRepository)
        {
            _administratorRepository = administratorRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var administrators = await _administratorRepository.GetAllAsync();
                return Ok(administrators);
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
                var administrator = await _administratorRepository.GetByIdAsync(id);
                return Ok(administrator);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AdministratorRequest request)
        {
            try
            {
                var administrator = await _administratorRepository.CreateAsync(request);
                return Ok(administrator);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] AdministratorRequest request)
        {
            try
            {
                var administrator = await _administratorRepository.UpdateAsync(id, request);
                return Ok(administrator);
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
                await _administratorRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
