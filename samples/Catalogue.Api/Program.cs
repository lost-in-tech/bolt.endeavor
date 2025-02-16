using Bolt.Endeavor.Extensions.Bus;
using Bolt.Endeavor.Extensions.Mvc;
using Bolt.IocScanner;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Environment.ApplicationName = "api-bookworm-catalogue";

builder.Host.UseSerilog((context, sp, logConfig) =>
{
    logConfig.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddMaySucceedForMvc<Program>(builder.Configuration);
builder.Services.AddRequestBus();
builder.Services.Scan<Program>(builder.Configuration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseDefaultLogScopes();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapEndpoints();
app.UseExceptionHandler("/");

app.Run();

public partial class Program{}