using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Core.Entities
{
    public class AnalyzeDocumentTableCell
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public string? Kind { get; set; }
        public string? Content { get; set; }
    }
}
