using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using PopOcr.Core.Entities;
using PopOcr.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ImageAnalysisResult result = await _client.AnalyzeAsync(
                BinaryData.FromStream(imageStream),
                VisualFeatures.Read);

            if (result.Read == null || !result.Read.Blocks.Any())
            {
                throw new Exception($"No text found in the image");
            }

            var text = string.Join(" ", result.Read.Blocks.SelectMany(block => block.Lines).Select(line => line.Text));
            return new OcrResults { Text = text };
        }

    }
}
