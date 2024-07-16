using PopOcr.Core.Entities;
using PopOcr.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Application.UseCases
{
    public class ExtractTextUseCase
    {
        private readonly IOcrService _ocrService;
        public ExtractTextUseCase(IOcrService ocrService)
        {
            _ocrService = ocrService;
        }

        public async Task<OcrResults> ExecuteAsync(Stream imageStream)
        {
            return await _ocrService.ExtractTextAsync(imageStream);
        }
    }
}
