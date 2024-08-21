using Microsoft.OpenApi.Models;
using PopOcr.Core.Interfaces;
using PopOcr.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
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

var cvKey = builder.Configuration["PopOcr:AzureCv_Key"];
var cvEndpoint = builder.Configuration["PopOcr:AzureCv_Endpoint"];
if (string.IsNullOrEmpty(cvEndpoint) || string.IsNullOrEmpty(cvKey))
{
    throw new InvalidOperationException("Azure Computer Vision endpoint and API key must be set.");
}

builder.Services.AddScoped<IFileGenerationService>(sp => new FileGenarationService());

builder.Services.AddScoped<IDocumentAnalysisService>(provider =>
{
    var fileGenarationService = provider.GetRequiredService<IFileGenerationService>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    var endpoint = configuration["PopOcr:Azure_Endpoint"];
    var apiKey = configuration["PopOcr:Azure_ApiKey"];
    if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(apiKey))
    {
        throw new InvalidOperationException("Azure Cognitive Services endpoint and APIKey must be set.");
    }

    return new DocumentAnalysisService(endpoint, apiKey, fileGenarationService);
});

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
