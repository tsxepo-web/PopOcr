using Azure.AI.DocumentIntelligence;
using Azure;
using PopOcr.Core.Interfaces;
using PopOcr.Core.Entities;
using Azure.Core.GeoJson;
using Microsoft.Extensions.Azure;
using System.Buffers.Text;

namespace PopOcr.Infrastructure.Services
{
    public class DocumentAnalysisService : IDocumentAnalysisService
    {
        private readonly string _endpoint;
        private readonly string _apiKey;
        private readonly DocumentIntelligenceClient _client;

        public DocumentAnalysisService(string endpoint, string apiKey)
        {
            _endpoint = endpoint;
            _apiKey = apiKey;
            _client = new DocumentIntelligenceClient(new Uri(_endpoint), new AzureKeyCredential(_apiKey));
        }

        public async Task<DocumentAnalysisResult> AnalyseDocumentAsync(string uriSource)
        {
            Uri uri = new(uriSource);
            var content = new AnalyzeDocumentContent() { UrlSource = uri };

            Operation<AnalyzeResult> operation = await _client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-layout", content);
            return ConvertResult(operation.Value);
        }

        public async Task<DocumentAnalysisResult> AnalyzeDocumentAsync(Stream imageStream)
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
        private DocumentAnalysisResult ConvertResult(AnalyzeResult result)
        {
            var documentAnalysisResult = new DocumentAnalysisResult
            {
                Pages = result.Pages.Select(page => new ExtractedPage
                {
                    PageNumber = page.PageNumber,
                    Lines = page.Lines.Select(line => new ExtractedLine
                    {
                        Content = line.Content,
                        BoundingPolygon = [.. line.Polygon]
                    }).ToList(),
                    SelectionMarks = page.SelectionMarks.Select(selectionMark => new ExtractedSelectionMark
                    {
                        State = selectionMark.State.ToString(),
                        BoundingPolygon = [.. selectionMark.Polygon]
                    }).ToList()
                }).ToList(),

                Styles = result.Styles.Select(style => new ExtractedStyle
                {
                    IsHandwritten = style.IsHandwritten,
                    Confidence = style.Confidence,  
                    Spans = style.Spans.Select(span => new ExtractedSpan
                    {
                        Index = span.Offset,
                        Length = span.Length,
                        Content = result.Content,
                    }).ToList(),    
                }).ToList(),

                Tables = result.Tables.Select(table => new ExtractedTable
                {
                    RowCount = table.RowCount,
                    ColumnCount = table.ColumnCount,
                    Cells = table.Cells.Select(cell => new TableCell
                    {
                        Content = cell.Content,
                        RowIndex = cell.RowIndex,
                        ColumnIndex = cell.ColumnIndex,
                        Kind = cell.Kind.ToString()
                    }).ToList()
                }).ToList(),
            };
            return documentAnalysisResult;
            
        }
    }
}
