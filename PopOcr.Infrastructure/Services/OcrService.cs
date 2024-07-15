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
        private readonly ComputerVisionClient _client;

        public OcrService(string endpoint, string apiKey)
        {
            _endpoint = endpoint;
            _apiKey = apiKey;
            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_apiKey))
            {
                Endpoint = _endpoint
            };
        }

        public async Task<OcrResult> ExtractTextAsync(Stream imageStream)
        {
            throw new NotImplementedException();
        }
    }
}
