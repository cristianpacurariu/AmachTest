using Amach.GetCreditData;
using Amach.HttpClients.CreditData;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Polly;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<IValidator<GetCreditDataRequest>, GetCreditDataRequestValidator>();

builder.Services
    .AddHttpClient<CreditDataHttpClient>(httpClient =>
    {
        httpClient.BaseAddress = new Uri(CreditDataHttpClient.Url);
    })
    .AddTransientHttpErrorPolicy(builder =>
    {
        return builder.WaitAndRetryAsync(
            3,
            retryNumber => TimeSpan.FromSeconds(Math.Pow(2, retryNumber)));
    });

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Lookup Service API",

        });

        options.ExampleFilters();

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    })
    .AddSwaggerExamplesFromAssemblyOf<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "lookup/api/{documentName}.json";
    });
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/lookup/api/v1.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();