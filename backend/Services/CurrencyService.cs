using backend.Configurations;
using backend.Models;
using backend.Repositories;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace backend.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IRepository _repository;
        private readonly HttpClient _httpClient;
        private readonly NbpApiSettings _apiSettings;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(IRepository repository, IHttpClientFactory httpClientFactory, IOptions<NbpApiSettings> apiSettings, ILogger<CurrencyService> logger)
        {
            _repository = repository;
            _httpClient = httpClientFactory.CreateClient("NBP");
            _apiSettings = apiSettings.Value;
            _logger = logger;
        }

        public async Task<IList<NbpRate>> GetCurrencyRatesAsync(CurrencyTable currencyTable = CurrencyTable.MainCurrencyTable)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var dataFromRepository = await GetRatesFromRepositoryAsync(today, currencyTable);
            if (dataFromRepository.Count != 0)
            {
                return dataFromRepository;
            }

            return await GetRatesFromApiAsync(currencyTable);
        }

        private async Task<IList<NbpRate>> GetRatesFromRepositoryAsync(DateOnly date, CurrencyTable currencyTable)
        {
            _logger.LogInformation($"Próba pobrania kursów walut z repozytorium dla tabeli {currencyTable}");

            var table = currencyTable switch
            {
                CurrencyTable.MainCurrencyTable => "A",
                CurrencyTable.MinorCurrencyTable => "B",
                _ => throw new ArgumentException($"Tabela '{currencyTable}' nie jest obsługiwana")
            };
            var today = DateOnly.FromDateTime(DateTime.Now);

            // tu mogłaby zostać zastosowana optymalizacja np. GetRatesByDateAndTableAsync(date, table)
            var existingRates = await _repository.GetAllAsync();
            var rates = existingRates
                .Where(e => e.EffectiveDate == date && e.Table == table)
                .SelectMany(e => e.Rates.Select(r => new NbpRate
                {
                    Currency = r.Currency,
                    Code = r.Code,
                    Mid = r.Mid
                })).ToList();

            return rates;
        }

        private async Task<IList<NbpRate>> GetRatesFromApiAsync(CurrencyTable currencyTable = CurrencyTable.MainCurrencyTable)
        {
            _logger.LogInformation($"Pobieranie kursów walut z API dla tabeli {currencyTable}");

            try
            {
                var apiAddress = currencyTable switch
                {
                    CurrencyTable.MainCurrencyTable => _apiSettings.AverageRatesMainCurrencies,
                    CurrencyTable.MinorCurrencyTable => _apiSettings.AverageRatesMinorCurrencies,
                    _ => throw new ArgumentException($"Tabela {currencyTable} nie jest obsługiwana przez API")
                };
                var mainCurrencyRates = await FetchRatesFromApiAsync(apiAddress);
                var repository = await _repository.GetAllAsync();

                // Sprawdzenie czy dane są już w repozytorium, aby nie dodawać ich ponownie.
                // Może być tak, że jest już kolejny dzień, ale API jeszcze odpowiada danymi z poprzedniego dnia, które już są dodane.
                // Zapytanie do API musi zostać wykonane, bo nie wiemy czy są już nowe dane. Jeśli będą nowe dane (z obecnego dnia) to
                // dane zostaną zapisane do repozytorium i ponowne zapytanie do API nie zostanie wykonane
                if (DataNotInRepository(mainCurrencyRates, repository))
                {
                    await _repository.AddAsync(mainCurrencyRates[0]);
                }
                else
                {
                    _logger.LogInformation("Dane z API odpowiadają już istniejącym w repozytorium, brak nowych danych.");
                }

                return mainCurrencyRates[0].Rates;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Błąd podczas pobierania danych z API NBP");
                throw new Exception("Błąd podczas pobierania danych z API NBP.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Nieoczekiwany błąd w GetRatesFromApiAsync");
                throw;
            }
        }

        private async Task<IList<NbpTable>> FetchRatesFromApiAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var contentStream = await response.Content.ReadAsStreamAsync();
            var nbpTableList = await JsonSerializer.DeserializeAsync<List<NbpTable>>(contentStream, options) ?? [];

            if (nbpTableList == null || nbpTableList.Count == 0)
            {
                _logger.LogWarning("API NBP zwróciło pustą listę danych.");
                throw new Exception("Brak danych z API NBP.");
            }

            return nbpTableList;
        }

        private static bool DataNotInRepository(IList<NbpTable> nbpTableList, IEnumerable<NbpTable> repository)
        {
            return !repository.Any(r => r.EffectiveDate == nbpTableList[0].EffectiveDate);
        }
    }
}