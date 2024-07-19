using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Core.Entities
{
    public class AnalyzeDocumentSelectionMark
    {
        public string? State { get; set; }
        public List<float>? Polygon { get; set; }
    }

}
