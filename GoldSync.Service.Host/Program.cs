using GoldSync.Service.Host.ProgramExtensions;
using GoldSync.Service.Host.ProgramExtensions.Application;
using GoldSync.Service.Host.ProgramExtensions.Builder;
using GoldSync.Service.Host.ProgramExtensions.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .ConfigureDbConnection(builder.Configuration)
    .AddDbContext();

builder.Services.AddFLows();

builder.Services
    .ConfigurePeriodicHostedService(builder.Configuration)
    .AddPeriodicHostedService();

builder.Services.AddCustomHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCriticalExceptionHandling();

app.OptionsValidate();

if (app.Environment.IsDevelopment() || app.Environment.IsLocal())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.MapGetBackgroundSettings("/backgroundSettings")
    .MapPostBackgroundSettings("/backgroundSettings")
    .MapGetServiceRestart("/restart");

app.Run();