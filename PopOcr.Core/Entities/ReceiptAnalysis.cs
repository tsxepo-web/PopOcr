public class ReceiptAnalysisResult
{
    public string? Status { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public DateTime? LastUpdatedDateTime { get; set; }
    public ReceiptAnalyzeResultInfo? AnalyzeResult { get; set; }
}

public class ReceiptAnalyzeResultInfo
{
    public string? ApiVersion { get; set; }
    public string? ModelId { get; set; }
    public string? StringIndexType { get; set; }
    public string? Content { get; set; }
    public List<ReceiptPage>? Pages { get; set; }
    public List<ReceiptStyle>? Styles { get; set; }
    public List<ReceiptDocument>? Documents { get; set; }
    public float? Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }  
    public string? ContentFormat { get; set; }  
}


public class ReceiptPage
{
    public int? PageNumber { get; set; }
    public float? Angle { get; set; }
    public float? Width { get; set; }
    public float? Height { get; set; }
    public string? Unit { get; set; }
    public List<ReceiptWord>? Words { get; set; }
    public List<ReceiptLine>? Lines { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}

public class ReceiptWord
{
    public string? Content { get; set; }
    public List<float>? Polygon { get; set; }
    public float? Confidence { get; set; }
    public ReceiptSpan? Span { get; set; }
}

public class ReceiptLine
{
    public string? Content { get; set; }
    public List<float>? Polygon { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}

public class ReceiptSpan
{
    public int? Offset { get; set; }
    public int? Length { get; set; }
}

public class ReceiptStyle
{
    public float Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
    public bool? IsHandwritten { get; set; }
}

public class ReceiptDocument
{
    public string? DocType { get; set; }
    public List<ReceiptBoundingRegion>? BoundingRegions { get; set; }
    public ReceiptFields? Fields { get; set; }
    public float Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}

public class ReceiptBoundingRegion
{
    public int PageNumber { get; set; }
    public List<float>? Polygon { get; set; }
}

public class ReceiptFields
{
    public ReceiptItems? Items { get; set; }
    public ReceiptMerchantAddress? MerchantAddress { get; set; }
    public ReceiptMerchantName? MerchantName { get; set; }
    public ReceiptSubtotal? Subtotal { get; set; }
    public ReceiptTip? Tip { get; set; }
    public ReceiptTotal? Total { get; set; }
    public ReceiptTotalTax? TotalTax { get; set; }
    public ReceiptTransactionDate? TransactionDate { get; set; }
    public ReceiptTransactionTime? TransactionTime { get; set; }
}

public class ReceiptItems
{
    public string? Type { get; set; }
    public List<ReceiptItem>? ValueArray { get; set; }
}

public class ReceiptItem
{
    public ReceiptDescription? Description { get; set; }
    public ReceiptQuantity? Quantity { get; set; }
    public ReceiptTotalPrice? TotalPrice { get; set; }
    public string? Content { get; set; }
    public List<ReceiptBoundingRegion>? BoundingRegions { get; set; }
    public float? Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}

public class ReceiptDescription
{
    public string? Type { get; set; }
    public string? ValueString { get; set; }
    public string? Content { get; set; }
    public List<ReceiptBoundingRegion>? BoundingRegions { get; set; }
    public float? Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}

public class ReceiptQuantity
{
    public string? Type { get; set; }
    public int ValueNumber { get; set; }
    public string? Content { get; set; }
    public List<ReceiptBoundingRegion>? BoundingRegions { get; set; }
    public float? Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}

public class ReceiptTotalPrice
{
    public string? Type { get; set; }
    public ReceiptCurrency? ValueCurrency { get; set; }
    public string? Content { get; set; }
    public List<ReceiptBoundingRegion>? BoundingRegions { get; set; }
    public float? Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}

public class ReceiptCurrency
{
    public string? CurrencySymbol { get; set; }
    public float? Amount { get; set; }
    public string? CurrencyCode { get; set; }
}

public class ReceiptMerchantAddress
{
    public string? Type { get; set; }
    public string? Content { get; set; }
    public List<ReceiptBoundingRegion>? BoundingRegions { get; set; }
    public float? Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
    public ReceiptAddress? ValueAddress { get; set; }
}

public class ReceiptAddress
{
    public string? HouseNumber { get; set; }
    public string? Road { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? CountryRegion { get; set; }
    public string? StreetAddress { get; set; }
}

public class ReceiptMerchantName
{
    public string? Type { get; set; }
    public string? ValueString { get; set; }
    public string? Content { get; set; }
    public List<ReceiptBoundingRegion>? BoundingRegions { get; set; }
    public float? Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}

public class ReceiptSubtotal
{
    public string? Type { get; set; }
    public ReceiptCurrency? ValueCurrency { get; set; }
    public string? Content { get; set; }
    public List<ReceiptBoundingRegion>? BoundingRegions { get; set; }
    public float? Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}

public class ReceiptTip
{
    public string? Type { get; set; }
    public ReceiptCurrency? ValueCurrency { get; set; }
    public string? Content { get; set; }
    public List<ReceiptBoundingRegion>? BoundingRegions { get; set; }
    public float? Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}

public class ReceiptTotal
{
    public string? Type { get; set; }
    public ReceiptCurrency? ValueCurrency { get; set; }
    public string? Content { get; set; }
    public List<ReceiptBoundingRegion>? BoundingRegions { get; set; }
    public float? Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}

public class ReceiptTotalTax
{
    public string? Type { get; set; }
    public ReceiptCurrency? ValueCurrency { get; set; }
    public string? Content { get; set; }
    public List<ReceiptBoundingRegion>? BoundingRegions { get; set; }
    public float? Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}

public class ReceiptTransactionDate
{
    public string? Type { get; set; }
    public string? ValueDate { get; set; }
    public string? Content { get; set; }
    public List<ReceiptBoundingRegion>? BoundingRegions { get; set; }
    public float? Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}

public class ReceiptTransactionTime
{
    public string? Type { get; set; }
    public string? ValueTime { get; set; }
    public string? Content { get; set; }
    public List<ReceiptBoundingRegion>? BoundingRegions { get; set; }
    public float? Confidence { get; set; }
    public List<ReceiptSpan>? Spans { get; set; }
}
