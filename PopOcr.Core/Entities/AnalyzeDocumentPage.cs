using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Core.Entities
{
    public class AnalyzeDocumentPage
    {
        public int PageNumber { get; set; }
        public List<AnalyzeDocumentLine>? Lines { get; set; }
        public List<AnalyzeDocumentSelectionMark>? SelectionMarks { get; set; }
        public List<AnalyzeDocumentWord>? Words { get; set; }

    }
}
