using PopOcr.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOpenXml;

namespace PopOcr.Infrastructure.Services
{
    public class FileGenarationService : IFileGenerationService
    {
        public async Task<byte[]> SaveTablesToExcelAsync(List<List<string>> tables)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Table");
                for (int i = 0; i < tables.Count; i++)
                {
                    for(int j = 0; j < tables[i].Count; j++)
                    {
                        worksheet.Cells[i + 1, j + 1].Value = tables[i][j];
                    }
                }
                return await package.GetAsByteArrayAsync();
            }
        }

        public Task<byte[]> SaveTextToWordAsync(string text)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(memoryStream, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
                {
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                    mainPart.Document = new Document();
                    Body body = mainPart.Document.AppendChild(new Body());
                    body.AppendChild(new Paragraph(new Run(new Text(text))));
                }
                return Task.FromResult(memoryStream.ToArray());
            }
        }
    }
}
