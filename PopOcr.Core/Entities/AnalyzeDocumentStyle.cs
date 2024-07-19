using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Core.Entities
{
    public class AnalyzeDocumentStyle
    {
        public bool? IsHandwritten { get; set; }
        public float Confidence { get; set; }
        public List<AnalyzeDocumentSpan>? Spans { get; set; }
    }
}
