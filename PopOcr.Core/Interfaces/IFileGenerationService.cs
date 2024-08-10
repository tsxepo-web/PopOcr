using PopOcr.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Core.Interfaces
{
    public interface IFileGenerationService
    {
        Task<byte[]> SaveTextToWordAsync(string text);
        Task<byte[]> SaveTablesToExcelAsync(List<ExtractedTable> tables);
    }
}
