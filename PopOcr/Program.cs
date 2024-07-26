using Microsoft.OpenApi.Models;
using PopOcr.Core.Interfaces;
using PopOcr.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OCR API", Version = "v1" });
});

var endpoint = builder.Configuration["PopOcr:Azure_Endpoint"];
var apiKey = builder.Configuration["PopOcr:Azure_ApiKey"];
if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(apiKey))
{
    throw new InvalidOperationException("Azure Cognitive Services endpoint and API key must be set.");
}

builder.Services.AddScoped<IOcrService>(sp => new OcrService(endpoint, apiKey));
builder.Services.AddScoped<IDocumentAnalysisService>(sp => new DocumentAnalysisService(endpoint, apiKey));
builder.Services.AddScoped<IReceiptAnalysisService>(sp => new ReceiptAnalysisService(endpoint, apiKey));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OCR API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
