using Azure.AI.DocumentIntelligence;
using Azure;
using PopOcr.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PopOcr.Core.Interfaces;

namespace PopOcr.Infrastructure.Services
{
    public class ReceiptAnalysisService : IReceiptAnalysisService
    {
        private readonly string _endpoint;
        private readonly string _apiKey;
        private readonly DocumentIntelligenceClient _client;

        public ReceiptAnalysisService(string endpoint, string apiKey)
        {
            _endpoint = endpoint;
            _apiKey = apiKey;
            _client = new DocumentIntelligenceClient(new Uri(_endpoint), new AzureKeyCredential(_apiKey));
        }

        public async Task<ReceiptAnalysis> AnalyseReceiptAsync(string uriSource)
        {
            Uri uri = new(uriSource);
            var content = new AnalyzeDocumentContent() { UrlSource = uri };

            Operation<AnalyzeResult> operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-layout", content);
            return ConvertResult(operation.Value);
        }

        public async Task<ReceiptAnalysis> AnalyzeReceiptAsync(Stream imageStream)
        {
            byte[] byteArray;
            using (var memoryStream = new MemoryStream())
            {
                await imageStream.CopyToAsync(memoryStream);
                byteArray = memoryStream.ToArray();
            }

            BinaryData data = new(byteArray);
            var content = new AnalyzeDocumentContent() { Base64Source = data };
            Operation<AnalyzeResult> operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-layout", content);
            return ConvertResult(operation.Value);
        }

        private ReceiptAnalysis ConvertResult(AnalyzeResult analyzeResult)
        {
            var customResult = new ReceiptAnalysis
            {
                AnalyzeResult = new ReceiptAnalyzeResultInfo
                {
                    ApiVersion = analyzeResult.ApiVersion?.ToString(),
                    ModelId = analyzeResult.ModelId,
                    StringIndexType = analyzeResult.StringIndexType.ToString(),
                    Content = analyzeResult.Content,
                    Pages = analyzeResult.Pages?.Select(page => new ReceiptPage
                    {
                        PageNumber = page.PageNumber,
                        Angle = page.Angle,
                        Width = page.Width,
                        Height = page.Height,
                        Unit = page.Unit.ToString(),
                        Words = page.Words?.Select(word => new ReceiptWord
                        {
                            Content = word.Content,
                            Polygon = (List<float>)word.Polygon,
                            Confidence = word.Confidence,
                            Span = new ReceiptSpan
                            {
                                Offset = word.Span.Offset,
                                Length = word.Span.Length
                            }
                        }).ToList()
                    }).ToList()
                }
            };

            return customResult;
        }
    }
}
