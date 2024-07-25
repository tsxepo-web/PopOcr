using PopOcr.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Core.Entities;
public class ReceiptAnalysis
{
    public string? Status { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public ReceiptAnalyzeResultInfo? AnalyzeResult { get; set; }
}

public class ReceiptAnalyzeResultInfo
{
    public string? ApiVersion { get; set; }
    public string? ModelId { get; set; }
    public string? StringIndexType { get; set; }
    public string? Content { get; set; }
    public List<ReceiptPage>? Pages { get; set; }
}

public class ReceiptPage
{
    public int PageNumber { get; set; }
    public float? Angle { get; set; }
    public float? Width { get; set; }
    public float? Height { get; set; }
    public string? Unit { get; set; }
    public List<ReceiptWord>? Words { get; set; }
}

public class ReceiptWord
{
    public string? Content { get; set; }
    public List<float>? Polygon { get; set; } = [];
    public float Confidence { get; set; }
    public ReceiptSpan? Span { get; set; }
}

public class ReceiptSpan
{
    public int Offset { get; set; }
    public int Length { get; set; }
}