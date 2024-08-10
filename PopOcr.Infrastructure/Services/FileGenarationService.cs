using PopOcr.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using OfficeOpenXml;
using PopOcr.Core.Entities;

namespace PopOcr.Infrastructure.Services
{
    public class FileGenarationService : IFileGenerationService
    {
        public async Task<byte[]> SaveTablesToExcelAsync(List<ExtractedTable> tables)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Tables");
            int currentRow = 1;
            foreach (var table in tables)
            {
                for (int rowIndex = 0; rowIndex < table.RowCount; rowIndex++)
                {
                    for (int colIndex = 0; colIndex < table.ColumnCount; colIndex++)
                    {
                        var cell = table.Cells?.FirstOrDefault(cell => cell.RowIndex == rowIndex && cell.ColumnIndex == colIndex);
                        if (cell != null)
                        {
                            worksheet.Cells[currentRow + rowIndex, colIndex + 1].Value = cell.Content;
                        }
                    }
                }
                currentRow += table.RowCount + 1;
            }
            return await Task.FromResult(package.GetAsByteArray());
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
