using Agria.WebApi.Template.Api.Routes;

using Template.WebApi;

using Template.WebApi.Configuration.Authentication;
using Template.WebApi.Configuration.Error;
using Template.WebApi.Configuration.Logging;
using Template.WebApi.Configuration.Security;
using Template.WebApi.Configuration.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.AddAuthenticationWithJwtBearer();
builder.AddOpenTelemetry(builder.Configuration);
builder.Services.AddCorsWithOrigins(builder.Environment, builder.Configuration);
builder.Services.AddApiVersioningAndExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(builder.Configuration);
builder.Services.AddExceptionHandler<DefaultExceptionHandler>();
builder.Services.AddAnimalTypeClient(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseSwaggerAndUI();

if (app.Environment.IsProduction())
    app.UseHsts();

app.UseCorrelationHeaderPropagationMiddleware();

app.UseHttpsRedirection();

app.UseCorsWithOrigins();

app.AddAnimalEndpoints(app.Configuration);

app.Run();
