using backend.Configurations;
using backend.Data;
using backend.Repositories;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Polly;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/log-.txt",
        rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext() // Dodaje kontekst logów, np. request ID
    .CreateLogger();

try
{
    Log.Information("Uruchamianie aplikacji");
    // Konfiguracja aplikacji
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    builder.Services.Configure<NbpApiSettings>(builder.Configuration.GetSection("NbpApiSettings"));
    builder.Services.AddSingleton<NbpHttpClientConfig>();
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    RegisterServices(builder.Services);

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

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate(); // Przeprowadza migrację bazy danych
    }

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
    Log.Information("Aplikacja uruchomiona pomyślnie");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplikacja zakończyła działanie z powodu krytycznego błędu");
}
finally
{
    Log.CloseAndFlush();
}

static void RegisterServices(IServiceCollection services)
{

    services.AddScoped<IRepository, EfRepository>();
    // services.AddSingleton<IRepository, InMemoryRepository>(); // opcjonalna podmiana na InMemoryRepository
    services.AddScoped<ICurrencyService, CurrencyService>();
    services.AddHttpClient("NBP", (sp, client) =>
    {
        var httpClientConfig = sp.GetRequiredService<NbpHttpClientConfig>();
        httpClientConfig.Configure(client);

    }).AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
}