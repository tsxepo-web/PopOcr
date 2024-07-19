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
            Uri uri = new Uri(uriSource);
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
        private  DocumentAnalysisResult ConvertResult(AnalyzeResult result)
        {
            var analysisResult = new DocumentAnalysisResult
            {
                Pages = result.Pages.Select(page => new AnalyzeDocumentPage
                {
                    PageNumber = page.PageNumber,
                    Lines = page.Lines.Select(line => new AnalyzeDocumentLine
                    {
                        Content = line.Content,
                        Polygon = line.Polygon.ToList()
                    }).ToList(),
                    SelectionMarks = page.SelectionMarks.Select(selectionMark => new AnalyzeDocumentSelectionMark
                    {
                        State = selectionMark.State.ToString(),
                        Polygon = selectionMark.Polygon.ToList()
                    }).ToList(),
                    Words = page.Words.Select(word => new AnalyzeDocumentWord
                    {
                        Content = word.Content
                    }).ToList()
                }).ToList(),
                Paragraphs = result.Paragraphs.Select(paragraph => new AnalyzeDocumentParagraph
                {
                    Content = paragraph.Content,
                    Role = paragraph.Role.ToString()
                }).ToList(),
                Styles = result.Styles.Select(style => new AnalyzeDocumentStyle
                {
                    IsHandwritten = style.IsHandwritten,
                    Confidence = style.Confidence,
                    Spans = style.Spans.Select(span => new AnalyzeDocumentSpan
                    {
                        Offset = span.Offset,
                        Length = span.Length
                    }).ToList()
                }).ToList(),
                Tables = result.Tables.Select(table => new AnalyzeDocumentTable
                {
                    RowCount = table.RowCount,
                    ColumnCount = table.ColumnCount,
                    Cells = table.Cells.Select(cell => new AnalyzeDocumentTableCell
                    {
                        RowIndex = cell.RowIndex,
                        ColumnIndex = cell.ColumnIndex,
                        Kind = cell.Kind.ToString(),
                        Content = cell.Content
                    }).ToList()
                }).ToList()
            };
            
            return analysisResult;
        }
    }
}
