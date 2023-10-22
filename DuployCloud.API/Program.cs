using Duplocloud.API.Configurations;
using Duplocloud.API.Datastore;
using Duplocloud.API.Datastore.Sqlite;
using Duplocloud.API.Models;
using Duplocloud.API.Services;
using Duplocloud.API.Services.OpenMeteo;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<IWeatherForecastRepository, SqliteWeatherForecastRepository>();
builder.Services.AddTransient<IWeatherForecastResolver, OpenMeteoForecastResolver>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddOptions();
builder.Services.Configure<OpenMeteoConfig>(builder.Configuration.GetSection("OpenMeteoConfig"));

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "Duplocloud.API.xml"));
});

// Configure the HTTP request pipeline.
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Exception handler
app.UseExceptionHandler(a => a.Run(async context =>
{
    var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    context.Response.ContentType = "application/json";

    string returnMessage = "Unexpected Error";
    int returnStatusCode = (int)HttpStatusCode.InternalServerError;
    if (error is InvalidRequestException)
    {
        returnStatusCode = (int)HttpStatusCode.BadRequest;
        returnMessage = error?.Message!;
    }

    context.Response.StatusCode = returnStatusCode;

    await context.Response.WriteAsJsonAsync(new FriendlyErrorResponse { Reason = returnMessage });
}));
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
