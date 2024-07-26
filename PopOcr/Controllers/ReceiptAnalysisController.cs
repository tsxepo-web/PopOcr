using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopOcr.Core.Entities;
using PopOcr.Core.Interfaces;
using PopOcr.Infrastructure.Services;

namespace PopOcr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptAnalysisController : ControllerBase
    {
        private readonly IReceiptAnalysisService _receiptAnalysisService;

        public ReceiptAnalysisController(IReceiptAnalysisService receiptAnalysisService)
        {
            _receiptAnalysisService = receiptAnalysisService;
        }

        [HttpPost("analyze_receipt")]
        public async Task<IActionResult> AnalyzeDocument([FromBody] AnalyzeDocumentReq request)
        {
            if (string.IsNullOrEmpty(request.UriSource))
            {
                return BadRequest("UriSource is required.");
            }
            try
            {
                ReceiptAnalysisResult result = await _receiptAnalysisService.AnalyseReceiptAsync(request.UriSource);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpPost("analyze-receipt")]
        public async Task<IActionResult> AnalyzeDocumentFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("A valid file is required.");
            }

            try
            {
                using var stream = file.OpenReadStream();
                ReceiptAnalysisResult result = await _receiptAnalysisService.AnalyzeReceiptAsync(stream);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        public class AnalyzeDocumentReq
        {
            public required string UriSource { get; set; }
        }
    }
}
