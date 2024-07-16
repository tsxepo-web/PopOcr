using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopOcr.Core.Entities;
using PopOcr.Core.Interfaces;

namespace PopOcr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopOcrController : ControllerBase
    {
        private readonly IOcrService _ocrService;

        public PopOcrController(IOcrService ocrService)
        {
            _ocrService = ocrService;
        }

        [HttpPost("extract-text")]
        public async Task<ActionResult<OcrResults>> ExtractText(IFormFile file)
        { 
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                using (var stream = file.OpenReadStream())
                {
                    var ocrResult = await _ocrService.ExtractTextAsync(stream);
                    return Ok(new { Text = ocrResult.Text });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error extracting text: {ex.Message}");
            }
        }
    }
}
