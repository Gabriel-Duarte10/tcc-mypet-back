using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tcc_mypet_back.Data.Interfaces;
using tcc_mypet_back.Data.Request;
using OfficeOpenXml;
using System.IO;
using tcc_mypet_back.Data.Dtos;

namespace tcc_mypet_back.Controllers
{
    //[[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetRepository _petRepository;

        public PetsController(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        [HttpGet("not-user/{userId}")]
        public async Task<IActionResult> GetAllAsync(int userId)
        {
            try
            {
                var pets = await _petRepository.GetAllAsync(userId);
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

        [HttpPut("favorite")]
        public async Task<IActionResult> RemoveFromFavoritesAsync([FromBody] FavoritePetRequest request)
        {
            try
            {
                await _petRepository.RemoveFromFavoritesAsync(request.PetId, request.UserId);
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

        [HttpPost("export-to-excel")]
        public async Task<IActionResult> ExportToExcel([FromBody] FilterModel filters)
        {
            OfficeOpenXml.ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            // Aqui você aplicaria os filtros e obteria os dados
            var pets = await _petRepository.GetFilteredPetsAsync(filters);

            // Criar o pacote Excel
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Pets");

            // Adicionar cabeçalhos
            int col = 1;
            worksheet.Cells[1, col++].Value = "ID";
            worksheet.Cells[1, col++].Value = "Name";
            worksheet.Cells[1, col++].Value = "BirthMonth";
            worksheet.Cells[1, col++].Value = "BirthYear";
            worksheet.Cells[1, col++].Value = "Gender";
            worksheet.Cells[1, col++].Value = "Description";
            worksheet.Cells[1, col++].Value = "IsNeutered";
            worksheet.Cells[1, col++].Value = "IsVaccinated";
            worksheet.Cells[1, col++].Value = "AdoptionStatus";
            worksheet.Cells[1, col++].Value = "CreatedAt";

            // Adicionando cabeçalhos do UserDto
            worksheet.Cells[1, col++].Value = "UserID";
            worksheet.Cells[1, col++].Value = "UserName";
            worksheet.Cells[1, col++].Value = "Cellphone";
            worksheet.Cells[1, col++].Value = "ZipCode";
            worksheet.Cells[1, col++].Value = "Street";
            worksheet.Cells[1, col++].Value = "Number";
            worksheet.Cells[1, col++].Value = "State";
            worksheet.Cells[1, col++].Value = "City";
            worksheet.Cells[1, col++].Value = "Longitude";
            worksheet.Cells[1, col++].Value = "Latitude";

            // Adicionar dados
            for (int i = 0; i < pets.Count; i++)
            {
                var pet = pets[i];
                col = 1;
                worksheet.Cells[i + 2, col++].Value = pet.Id;
                worksheet.Cells[i + 2, col++].Value = pet.Name;
                worksheet.Cells[i + 2, col++].Value = pet.BirthMonth;
                worksheet.Cells[i + 2, col++].Value = pet.BirthYear;
                worksheet.Cells[i + 2, col++].Value = pet.Gender ? "Male" : "Female";
                worksheet.Cells[i + 2, col++].Value = pet.Description;
                worksheet.Cells[i + 2, col++].Value = pet.IsNeutered;
                worksheet.Cells[i + 2, col++].Value = pet.IsVaccinated;
                worksheet.Cells[i + 2, col++].Value = pet.AdoptionStatus;
                worksheet.Cells[i + 2, col++].Value = pet.CreatedAt?.ToString("yyyy-MM-dd");

                // Dados do UserDto
                worksheet.Cells[i + 2, col++].Value = pet.User.Id;
                worksheet.Cells[i + 2, col++].Value = pet.User.Name;
                worksheet.Cells[i + 2, col++].Value = pet.User.Cellphone;
                worksheet.Cells[i + 2, col++].Value = pet.User.ZipCode;
                worksheet.Cells[i + 2, col++].Value = pet.User.Street;
                worksheet.Cells[i + 2, col++].Value = pet.User.Number;
                worksheet.Cells[i + 2, col++].Value = pet.User.State;
                worksheet.Cells[i + 2, col++].Value = pet.User.City;
                worksheet.Cells[i + 2, col++].Value = pet.User.Longitude;
                worksheet.Cells[i + 2, col++].Value = pet.User.Latitude;
            }

            // Converter para array de bytes e retornar como arquivo
            var stream = new MemoryStream(package.GetAsByteArray());
            return new FileStreamResult(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "PetsAndUsers.xlsx"
            };

        }
    }
}