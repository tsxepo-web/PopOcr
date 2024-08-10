using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopOcr.Core.Entities;
using PopOcr.Core.Interfaces;
using PopOcr.Infrastructure.Services;

namespace PopOcr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentAnalysisController : ControllerBase
    {
        private readonly IDocumentAnalysisService _documentAnalysisService;

        public DocumentAnalysisController(IDocumentAnalysisService documentAnalysisService)
        {
            _documentAnalysisService = documentAnalysisService;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeDocument([FromBody] AnalyzeDocumentRequest request)
        {
            if (string.IsNullOrEmpty(request.UriSource))
            {
                return BadRequest("UriSource is required.");
            }
            try
            {
                DocumentAnalysisResult result = await _documentAnalysisService.AnalyseDocumentAsync(request.UriSource);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        [HttpPost("analyze-file")]
        public async Task<IActionResult> AnalyzeDocumentFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("A valid file is required.");
            }

            try
            {
                using var stream = file.OpenReadStream();
                DocumentAnalysisResult result = await _documentAnalysisService.AnalyzeDocumentAsync(stream);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        public class AnalyzeDocumentRequest
        {
            public required string UriSource { get; set; }
        }

        [HttpPost("save-text-to-word")]
        public async Task<IActionResult> SaveTextToWord([FromBody] string text)
        {
            var fileBytes = await _documentAnalysisService.SaveExtractedTextToWordAsync(text);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "extractedText.docx");
        }

        [HttpPost("save-tables-to-excel")]
        public async Task<IActionResult> SaveTablesToExcel([FromBody] List<List<string>> tables)
        {
            var fileBytes = await _documentAnalysisService.SaveTablesToExcelAsync(tables);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "extractedTables.xlsx");
        }
    }
}
