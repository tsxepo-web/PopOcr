using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Core.Entities
{
    public class AnalyzeDocumentTable
    {
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }
        public List<AnalyzeDocumentTableCell>? Cells { get; set; }
    }
}
