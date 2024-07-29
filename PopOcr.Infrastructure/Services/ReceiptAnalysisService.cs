using Azure.AI.DocumentIntelligence;
using Azure;
using PopOcr.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PopOcr.Core.Interfaces;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Reflection.Metadata;

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

        public async Task<ReceiptAnalysisResult> AnalyseReceiptAsync(string uriSource)
        {
            Uri uri = new(uriSource);
            var content = new AnalyzeDocumentContent() { UrlSource = uri };

            Operation<AnalyzeResult> operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-layout", content);
            return ConvertResult(operation.Value);
        }

        public async Task<ReceiptAnalysisResult> AnalyzeReceiptAsync(Stream imageStream)
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

        private ReceiptAnalysisResult ConvertResult(AnalyzeResult analyzeResult)
        {
            var analyzeResultInfo = new ReceiptAnalyzeResultInfo
            {
                ApiVersion = analyzeResult.ApiVersion,
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
                        Polygon = word.Polygon?.ToList(),
                        Confidence = word.Confidence,
                        Span = word.Span != null ? new ReceiptSpan
                        {
                            Offset = word.Span.Offset,
                            Length = word.Span.Length
                        } : null
                    }).ToList(),
                    Lines = page.Lines?.Select(line => new ReceiptLine
                    {
                        Content = line.Content,
                        Polygon = line.Polygon?.ToList(),
                        Spans = line.Spans?.Select(span => new ReceiptSpan
                        {
                            Offset = span.Offset,
                            Length = span.Length
                        }).ToList()
                    }).ToList(),
                    Spans = page.Spans?.Select(span => new ReceiptSpan
                    {
                        Offset = span.Offset,
                        Length = span.Length
                    }).ToList()
                }).ToList(),

                Styles = analyzeResult.Styles?.Select(style => new ReceiptStyle
                {
                    Confidence = style.Confidence,
                    Spans = style.Spans?.Select(span => new ReceiptSpan
                    {
                        Offset = span.Offset,
                        Length = span.Length
                    }).ToList(),
                    IsHandwritten = style.IsHandwritten
                }).ToList(),
                ContentFormat = analyzeResult.ContentFormat.ToString()
            };

            var receiptAnalysisResult = new ReceiptAnalysisResult
            {
                AnalyzeResult = analyzeResultInfo
            };

            return receiptAnalysisResult;
        }

    }
}
