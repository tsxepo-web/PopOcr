using PopOcr.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Core.Interfaces
{
    public interface IDocumentAnalysisService
    {
       Task<DocumentAnalysisResult> AnalyseDocumentAsync(string uriSource);
        Task<DocumentAnalysisResult> AnalyzeDocumentAsync(Stream imageStream);
    }
}
