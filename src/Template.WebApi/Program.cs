using Agria.WebApi.Template.Api.Routes;

using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

using Template.WebApi.Configuration.Authentication;
using Template.WebApi.Configuration.Error;
using Template.WebApi.Configuration.Logging;
using Template.WebApi.Configuration.Security;
using Template.WebApi.Configuration.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.AddAuthenticationWithJwtBearer();
builder.AddOpenTelemetry(builder.Configuration);
builder.Services.AddCorsWithOrigins(builder.Environment, builder.Configuration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(builder.Configuration);
builder.Services.AddExceptionHandler<DefaultExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseSwaggerAndUI();

app.UseCorrelationHeaderPropagationMiddleware();

app.UseHttpsRedirection();

app.UseCorsWithOrigins();

app.AddAnimalEndpoints();

app.Run();
