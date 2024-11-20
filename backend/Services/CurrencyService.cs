using backend.Models;
using backend.Repositories;
using System.Text.Json;

namespace backend.Services
{
    public class CurrencyService
    {
        private readonly IRepository _repository;
        private readonly HttpClient _httpClient;

        public CurrencyService(IRepository repository, IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _httpClient = httpClientFactory.CreateClient("NBP");
        }

        public async Task<List<NbpRate>> GetTodayRatesFromRepositoryAsync()
        {
            var existingRates = await _repository.GetAllAsync();
            var today = DateOnly.FromDateTime(DateTime.Now).ToString();
            var todayRates = existingRates
                .Where(e => e.EffectiveDate == today)
                .SelectMany(e => e.Rates)
                .ToList();

            return todayRates;
        }

        public async Task<List<NbpRate>?> GetRatesFromApiAsync()
        {
            var rates = await GetCurrencyRatesAsync();

            if (rates == null || !rates.Any())
            {
                return null;
            }

            var rate = rates.First();
            var nbpTable = new NbpTable
            {
                Table = rate.Table,
                No = rate.No,
                EffectiveDate = rate.EffectiveDate,
                Rates = rate.Rates.Select(r => new NbpRate
                {
                    Currency = r.Currency,
                    Code = r.Code,
                    Mid = r.Mid
                }).ToList()
            };

            await _repository.AddAsync(nbpTable);
            return nbpTable.Rates;
        }

        public async Task<List<NbpTable>> GetCurrencyRatesAsync()
        {
            var apiUrl = "https://api.nbp.pl/api/exchangerates/tables/A/?format=json";
            try
            {
                var response = await _httpClient.GetAsync(apiUrl);

                // Sprawdzenie statusu odpowiedzi
                response.EnsureSuccessStatusCode();

                // Deserializacja danych
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var contentStream = await response.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync<List<NbpTable>>(contentStream, options) ?? [];
            }
            catch (HttpRequestException ex)
            {
                // Logowanie błędu (przykład z konsolą - można użyć np. ILogger)
                // Console.WriteLine($"Błąd żądania HTTP: {ex.Message}");
                throw new Exception("Błąd podczas pobierania danych z API NBP.", ex);
            }
            catch (Exception ex)
            {
                // Logowanie innych błędów
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
                throw;
            }
        }
    }
}