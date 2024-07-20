using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopOcr.Core.Entities;
public class DocumentAnalysisResult
{
    public List<ExtractedPage> Pages { get; set; } = [];
    public List<ExtractedStyle> Styles { get; set; } = [];
    public List<ExtractedTable> Tables { get; set; } = [];
}

public class ExtractedPage
{
    public int PageNumber { get; set; }
    public List<ExtractedLine> Lines { get; set; } = [];
    public List<ExtractedSelectionMark> SelectionMarks { get; set; } = [];
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
    public double Confidence { get; set; }
    public List<ExtractedSpan> Spans { get; set; } = [];
}

public class ExtractedSpan
{
    public int Index { get; set; }
    public int Length { get; set; }
    public string? Content { get; set; }
}

public class ExtractedTable
{
    public int RowCount { get; set; }
    public int ColumnCount { get; set; }
    public List<TableCell> Cells { get; set; } = [];
}

public class TableCell
{
    public string? Content { get; set; }
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public string? Kind { get; set; }
}


