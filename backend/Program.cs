using backend.Configurations;
using backend.Repositories;
using backend.Services;
using Polly;

// Konfiguracja aplikacji
var builder = WebApplication.CreateBuilder(args);
RegisterServices(builder.Services);

builder.Services.AddSingleton<AppSettingsConfig>();

builder.Services.AddControllers();

// Swagger i OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("https://localhost:3000", "https://tlmap.com")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Logging.AddConsole();  // Włączenie logowania do konsoli

var app = builder.Build();

app.UseStaticFiles();  // Serwowanie plików statycznych (w tym przypadku plików Reacta)
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowReactApp");

// Obsługa routingu i kontrolerów
app.MapControllers();

// Konfiguracja dla środowiska deweloperskiego (Swagger)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Obsługa React
app.MapFallbackToFile("index.html");  // Przekierowuje na index.html aplikacji React, jeśli inne ścieżki nie pasują

app.Use(async (context, next) =>
{
    Console.WriteLine("Request path: " + context.Request.Path); // Sprawdź, która ścieżka jest przetwarzana
    await next();
});

app.Run();

static void RegisterServices(IServiceCollection services)
{
    services.AddSingleton<IRepository, InMemoryRepository>();
    services.AddScoped<ICurrencyService, CurrencyService>();
    services.AddHttpClient("NBP", HttpClientConfig.Configure).AddTransientHttpErrorPolicy(policy =>
            policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))); // aby powtarzał zapytanie
}