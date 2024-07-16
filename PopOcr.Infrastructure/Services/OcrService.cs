using Azure.AI.Vision.ImageAnalysis;
using PopOcr.Core.Entities;
using PopOcr.Core.Exceptions;
using PopOcr.Core.Interfaces;

namespace PopOcr.Infrastructure.Services
{
    public class OcrService : IOcrService
    {
        private readonly string _endpoint;
        private readonly string _apiKey;
        private readonly ImageAnalysisClient _client;

        public OcrService(string endpoint, string apiKey)
        {
            _endpoint = endpoint;
            _apiKey = apiKey;
            _client = new ImageAnalysisClient(new Uri(_endpoint), new Azure.AzureKeyCredential(_apiKey));
        }
        public async Task<OcrResults> ExtractTextAsync(Stream imageStream)
        {
            try
            {
                ImageAnalysisResult result = await _client.AnalyzeAsync(
                    BinaryData.FromStream(imageStream),
                    VisualFeatures.Read);

                if (result.Read == null || !result.Read.Blocks.Any())
                {
                    throw new OcrException($"No text found in the image");
                }

                var text = string.Join(" ", result.Read.Blocks.SelectMany(block => block.Lines).Select(line => line.Text));
                return new OcrResults { Text = text };
            }
            catch (Exception ex)
            {
                throw new OcrException($"An error occured while extracting text: {ex.Message}");
            }
        }

    }
}
