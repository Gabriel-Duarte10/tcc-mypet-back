using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Controllers
{
    //[[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return Ok(products);
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
                var product = await _productRepository.GetByIdAsync(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetProductsByUserIdAsync(int userId)
        {
            try
            {
                var products = await _productRepository.GetProductsByUserIdAsync(userId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] ProductRequest request)
        {
            try
            {
                var product = await _productRepository.CreateAsync(request);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] ProductRequest request)
        {
            try
            {
                var product = await _productRepository.UpdateAsync(id, request);
                return Ok(product);
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
                await _productRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("favorite")]
        public async Task<IActionResult> AddToFavoriteAsync([FromBody] FavoriteProductRequest request)
        {
            try
            {
                var favorite = await _productRepository.AddToFavoriteAsync(request);
                return Ok(favorite);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("favorite")]
        public async Task<IActionResult> RemoveFromFavoritesAsync([FromQuery] int productId, [FromQuery] int userId)
        {
            try
            {
                await _productRepository.RemoveFromFavoritesAsync(productId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("report")]
        public async Task<IActionResult> ReportProductAsync([FromBody] ReportedProductRequest request)
        {
            try
            {
                var report = await _productRepository.ReportProductAsync(request);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("report")]
        public async Task<IActionResult> UnreportProductAsync([FromQuery] int productId)
        {
            try
            {
                await _productRepository.UnreportProductAsync(productId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("favorites")]
        public async Task<IActionResult> GetAllFavoriteProductsAsync()
        {
            try
            {
                var favorites = await _productRepository.GetAllFavoriteProductsAsync();
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("favorites/{userId}")]
        public async Task<IActionResult> GetFavoriteProductsByUserIdAsync(int userId)
        {
            try
            {
                var favorites = await _productRepository.GetFavoriteProductsByUserIdAsync(userId);
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("reported/user/{userId}")]
        public async Task<IActionResult> GetReportedProductsByUserIdAsync(int userId)
        {
            try
            {
                var reportedProducts = await _productRepository.GetReportedProductsByUserIdAsync(userId);
                return Ok(reportedProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("reported")]
        public async Task<IActionResult> GetAllReportedProductsAsync()
        {
            try
            {
                var reportedProducts = await _productRepository.GetAllReportedProductsAsync();
                return Ok(reportedProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}