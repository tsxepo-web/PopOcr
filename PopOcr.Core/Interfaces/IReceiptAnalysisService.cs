using PopOcr.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Core.Interfaces
{
    public interface IReceiptAnalysisService
    {

        Task<ReceiptAnalysisResult> AnalyseReceiptAsync(string uriSource);
        Task<ReceiptAnalysisResult> AnalyzeReceiptAsync(Stream imageStream);
    }
}
