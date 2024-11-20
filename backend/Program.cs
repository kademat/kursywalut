using backend.Configurations;
using backend.Models;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja usług
builder.Services.AddScoped<CurrencyService>();

builder.Services.AddHttpClient("NBP", client =>
{
    HttpClientConfig.Configure(client);  // Wywołanie statycznej metody Configure
});

builder.Services.AddSingleton<ICurrencyRateRepository, InMemoryCurrencyRateRepository>();

builder.Services.AddControllers();

// Swagger i OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("https://localhost:3000", "https://tlmap.com")  // Adres aplikacji React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Włączenie logowania
builder.Logging.AddConsole();  // Włączenie logowania do konsoli

var app = builder.Build();

// Konfiguracja CORS
app.UseCors("AllowReactApp");

// Obsługa routingu i kontrolerów
app.UseRouting();
app.MapControllers();

// Konfiguracja dla środowiska deweloperskiego (Swagger)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Umożliwienie obsługi plików statycznych (React)
app.UseStaticFiles();  // Serwowanie plików statycznych (plików Reacta)

app.UseRouting();

// Obsługa SPA (React)
app.MapFallbackToFile("index.html");  // Przekierowuje na index.html aplikacji React, jeśli inne ścieżki nie pasują

app.UseHttpsRedirection();
app.Run();