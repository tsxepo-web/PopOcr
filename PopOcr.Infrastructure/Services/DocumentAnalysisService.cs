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
        private readonly IFileGenerationService _fileGenerationService;

        public DocumentAnalysisService(string endpoint, string apiKey, IFileGenerationService fileGenerationService)
        {
            _endpoint = endpoint;
            _apiKey = apiKey;
            _client = new DocumentIntelligenceClient(new Uri(_endpoint), new AzureKeyCredential(_apiKey));
            _fileGenerationService = fileGenerationService;
        }
        private DocumentAnalysisResult ConvertResult(AnalyzeResult result)
        {
            var documentAnalysisResult = new DocumentAnalysisResult
            {
                AnalyzeResult = new AnalyzeResultInfo
                {
                    ApiVersion = result.ApiVersion,
                    ModelId = result.ModelId,
                    StringIndexType = result.StringIndexType.ToString(),
                    Content = result.Content,
                    Pages = result.Pages.Select(page => new ExtractedPage
                    {
                        PageNumber = page.PageNumber,
                        Angle = page.Angle,
                        Width = page.Width,
                        Height = page.Height,
                        Unit = page.Unit.ToString(),
                        Words = page.Words.Select(word => new ExtractedWord
                        {
                            Content = word.Content,
                            Polygon = (List<float>)word.Polygon,
                            Confidence = word.Confidence,
                            Span = new ExtractedSpan
                            {
                                Offset = word.Span.Offset,
                                Length = word.Span.Length
                            }
                        }).ToList()
                    }).ToList(),

                    Tables = result.Tables?.Select(table => new ExtractedTable
                    {
                        RowCount = table.RowCount,
                        ColumnCount = table.ColumnCount,    
                        Cells = table.Cells.Select(cell => new ExtractedTableCell
                        {
                            Kind = cell.Kind.ToString(),
                            RowIndex = cell.RowIndex,
                            ColumnIndex = cell.ColumnIndex,
                            Content = cell.Content,
                            BoundingRegions = cell.BoundingRegions?.Select(br => new ExtractedBoundingRegion
                            {
                                PageNumber = br.PageNumber,
                                Polygon = (List<float>)br.Polygon,
                            }).ToList(),
                            Spans = cell.Spans.Select(span => new ExtractedSpan
                            {
                                Offset = span.Offset,
                                Length = span.Length
                            }).ToList(),
                            Elements = (List<string>)cell.Elements
                        }).ToList(),
                        BoundingRegions = table.BoundingRegions.Select(br => new ExtractedBoundingRegion
                        {
                            PageNumber= br.PageNumber,
                            Polygon= (List<float>)br.Polygon,
                        }).ToList(),
                        Spans = table.Spans.Select(span => new ExtractedSpan
                        {
                            Offset= span.Offset,
                            Length = span.Length,
                        }).ToList(),
                        Caption = table.Caption != null ? new ExtractedTableCaption
                        {
                            Content = table.Caption.Content,
                            BoundingRegions = table.Caption.BoundingRegions.Select(br => new ExtractedBoundingRegion
                            {
                                PageNumber = br.PageNumber,
                                Polygon = (List<float>)br.Polygon,
                            }).ToList(),
                            Spans = table.Caption.Spans.Select(span => new ExtractedSpan
                            {
                                Offset = span.Offset,
                                Length = span.Length,
                            }).ToList(),
                            Elements = (List<string>)table.Caption.Elements
                        } : null
                    }).ToList(),
                    Paragraphs = result.Paragraphs.Select(paragraph => new ExtactedParagraph
                    {
                        Role = paragraph.Role.ToString(),
                        Content = paragraph.Content,
                        Spans = paragraph.Spans.Select(span => new ExtractedSpan
                        {
                            Offset = span.Offset,
                            Length = span.Length,
                        }).ToList(),
                        BoundingRegions = paragraph.BoundingRegions.Select(br => new ExtractedBoundingRegion
                        {
                            PageNumber= br.PageNumber,
                            Polygon = (List<float>)br.Polygon
                        }).ToList(),
                    }).ToList(),
                    Sections = result.Sections.Select(section => new ExtractedSection
                    {
                        Spans = section.Spans.Select(span => new ExtractedSpan
                        {
                            Offset = span.Offset,
                            Length = span.Length,
                        }).ToList(),
                        Elements = (List<string>)section.Elements
                    }).ToList(),
                    Figures = result.Figures.Select(figure => new ExtractedFigure
                    {
                        Id = figure.ToString(),
                        BoundingRegions = figure.BoundingRegions.Select(br => new ExtractedBoundingRegion
                        {
                            PageNumber = br.PageNumber,
                            Polygon = (List<float>)br.Polygon
                        }).ToList(),
                        Spans = figure.Spans.Select(span => new ExtractedSpan
                        {
                            Offset= span.Offset,
                            Length = span.Length,
                        }).ToList(),
                        Elements = (List<string>)figure.Elements,
                        Caption = figure.Caption != null ? new ExtractedFigureCaption
                        {
                            Content = figure.Caption.Content,
                            BoundingRegions = figure.Caption.BoundingRegions.Select(br => new ExtractedBoundingRegion
                            {
                                PageNumber = br.PageNumber,
                                Polygon= (List<float>)br.Polygon
                            }).ToList(),
                            Spans = figure.Caption.Spans.Select(span => new ExtractedSpan
                            {
                                Offset = span.Offset,
                                Length = span.Length,
                            }).ToList(),
                            Elements = (List<string>)figure.Caption.Elements
                        } : null
                    }).ToList()
                }
            };
            return documentAnalysisResult;   
        }
        public async Task<byte[]> SaveExtractedTextToWordAsync(string extractedText)
        {
            return await _fileGenerationService.SaveTextToWordAsync(extractedText);
        }

        public async Task<byte[]> SaveTablesToExcelAsync(List<ExtractedTable> tables)
        {
            return await _fileGenerationService.SaveTablesToExcelAsync(tables);
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
    }
}
