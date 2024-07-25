using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Core.Entities;
public class DocumentAnalysisResult
{
    public string? Status { get; set; }    
    public DateTime? CreatedDate { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public AnalyzeResultInfo? AnalyzeResult {  get; set; }
}

public class AnalyzeResultInfo
{
    public string? ApiVersion { get; set; }
    public string? ModelId { get; set; }
    public string? StringIndexType { get; set; }
    public string? Content { get; set; }
    public List<ExtractedPage>? Pages { get; set; }
    public List<ExtractedTable>? Tables { get; set; }
    public List<ExtactedParagraph>? Paragraphs { get; set; }
}

public class ExtractedPage
{
    public int PageNumber { get; set; }
    public float? Angle { get; set; }
    public float? Width { get; set; }
    public float? Height { get; set; }
    public string? Unit { get; set; }
    public List<ExtractedWord>? Words { get; set; }
}

public class ExtractedWord
{
    public string? Content { get; set; }
    public List<float>? Polygon { get; set; } = [];
    public float Confidence { get; set; }
    public ExtractedSpan? Span { get; set; }
}

public class ExtractedLine
{
    public string? Content { get; set; }
    public IList<float> BoundingPolygon { get; set; } = [];
}

public class ExtractedSelectionMark
{
    public string? State { get; set; }
    public IList<float> BoundingPolygon { get; set; } = [];
}

public class ExtractedStyle
{
    public bool? IsHandwritten { get; set; }
    public float Confidence { get; set; }
    public List<ExtractedSpan> Spans { get; set; } = [];
}

public class ExtractedSpan
{
    public int Offset { get; set; }
    public int Length { get; set; }
}

public class ExtractedTable
{
    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
    public List<ExtractedTableCell>? Cells { get; set; }
    public List<ExtractedBoundingRegion>? BoundingRegions { get; set; }
    public List<ExtractedSpan> Spans { get; set; } = [];
    public ExtractedTableCaption? Caption { get; set; }
}


public class ExtractedTableCell
{
    public string? Kind { get; set; }
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public string? Content { get; set; }
    public List<ExtractedBoundingRegion>? BoundingRegions { get; set; }
    public List<ExtractedSpan>? Spans { get; set; }
    public List<string>? Elements { get; set; }
}

public class ExtractedBoundingRegion
{
    public int PageNumber { get; set; }
    public List<float>? Polygon { get; set; }
}

public class ExtractedTableCaption
{
    public string? Content { get; set; }
    public List<ExtractedBoundingRegion>? BoundingRegions { get; set; }
    public List<ExtractedSpan>? Spans { get; set; }
    public List<string>? Elements { get; set; }
}

public class ExtactedParagraph
{
    public string? Role { get; set; }
    public string? Content { get; set; }
    public List<ExtractedSpan>? Spans { get; set; }
    public List<ExtractedBoundingRegion>? BoundingRegions { get; set; }
}