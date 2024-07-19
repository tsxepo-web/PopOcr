using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Core.Entities
{
    public class AnalyzeDocumentLine
    {
        public string? Content { get; set; }
        public List<float>? Polygon { get; set; }
    }
}
