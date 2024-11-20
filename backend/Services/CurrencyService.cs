using backend.Models;
using backend.Repositories;
using System.Text.Json;

namespace backend.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IRepository _repository;
        private readonly HttpClient _httpClient;

        public CurrencyService(IRepository repository, IHttpClientFactory httpClientFactory)
        {
            _repository = repository;
            _httpClient = httpClientFactory.CreateClient("NBP");
        }

        public async Task<List<NbpTable>> GetCurrencyRatesAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var dataFromRepository = await GetRatesFromRepositoryAsync(today);
            if (dataFromRepository.Count != 0)
            {
                return dataFromRepository;
            }

            return await GetRatesFromApiAsync();
        }

        private async Task<List<NbpTable>> GetRatesFromRepositoryAsync(DateOnly date)
        {
            var existingRates = await _repository.GetAllAsync();
            var today = DateOnly.FromDateTime(DateTime.Now);

            var todayTables = existingRates
                .Where(e => e.EffectiveDate == date)
                .Select(e => new NbpTable
                {
                    Table = e.Table,
                    No = e.No,
                    EffectiveDate = e.EffectiveDate,
                    Rates = e.Rates.Select(r => new NbpRate
                    {
                        Currency = r.Currency,
                        Code = r.Code,
                        Mid = r.Mid
                    }).ToList()
                })
                .ToList();

            return todayTables;
        }

        private async Task<List<NbpTable>> GetRatesFromApiAsync()
        {
            var apiUrl = "api/exchangerates/tables/A/?format=json";

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
                // Tu warto by było dodać logowanie do pliku lub bazy danych
                throw new Exception("Błąd podczas pobierania danych z API NBP.", ex);
            }
            catch (Exception ex)
            {
                // Tu warto by było dodać logowanie do pliku lub bazy danych - inny błąd
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
                throw;
            }
        }
    }
}