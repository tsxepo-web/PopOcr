using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Core.Entities
{
    public class DocumentAnalysisResult
    {
        public List<AnalyzeDocumentPage>? Pages {  get; set; }
        public List<AnalyzeDocumentParagraph>? Paragraphs { get; set; }
        public List<AnalyzeDocumentStyle>? Styles { get; set; }
        public List<AnalyzeDocumentTable>? Tables { get; set; }
    }
}
