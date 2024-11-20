using backend.Configurations;
using backend.Repositories;
using backend.Services;
using Polly;

// Konfiguracja aplikacji
var builder = WebApplication.CreateBuilder(args);
RegisterServices(builder.Services);
builder.Services.AddControllers();

// Swagger i OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", CorsConfig.ConfigureAllowReactAppPolicy);
});

// Włączenie logowania
builder.Logging.AddConsole();  // Włączenie logowania do konsoli

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();  // Serwowanie plików statycznych (w tym przypadku plików Reacta)
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

app.Run();

static void RegisterServices(IServiceCollection services)
{
    services.AddScoped<CurrencyService>();
    services.AddSingleton<IRepository, InMemoryRepository>();
    services.AddHttpClient("NBP", client =>
    {
        HttpClientConfig.Configure(client);
    })
        .AddTransientHttpErrorPolicy(policy =>
            policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))); ;
}