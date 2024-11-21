using backend.Models;
using backend.Repositories;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<IList<NbpRate>> GetCurrencyRatesAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var dataFromRepository = await GetRatesFromRepositoryAsync(today);
            if (dataFromRepository.Count != 0)
            {
                return dataFromRepository;
            }

            return await GetRatesFromApiAsync();
        }

        private async Task<IList<NbpRate>> GetRatesFromRepositoryAsync(DateOnly date)
        {
            var existingRates = await _repository.GetAllAsync();
            var today = DateOnly.FromDateTime(DateTime.Now);

            var rates = existingRates
                .Where(e => e.EffectiveDate == date)
                .SelectMany(e => e.Rates.Select(r => new NbpRate
                {
                    Currency = r.Currency,
                    Code = r.Code,
                    Mid = r.Mid
                })).ToList();

            return rates;
        }

        private async Task<IList<NbpRate>> GetRatesFromApiAsync()
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

                var nbpTableList = await JsonSerializer.DeserializeAsync<List<NbpTable>>(contentStream, options) ?? [];

                if (nbpTableList == null || nbpTableList.Count == 0)
                {
                    return [];
                }

                var repository = await _repository.GetAllAsync();

                // Sprawdzenie czy dane są już w repozytorium, aby nie dodawać ich ponownie.
                // Może być tak, że jest już kolejny dzień, ale API jeszcze odpowiada danymi z poprzedniego dnia, które już są dodane.
                // Zapytanie do API musi zostać wykonane, bo nie wiemy czy są już nowe dane. Jeśli będą nowe dane (z obecnego dnia) to
                // dane zostaną zapisane do repozytorium i ponowne zapytanie do API nie zostanie wykonane
                if (DataNotInRepository(nbpTableList, repository))
                {
                    await _repository.AddAsync(nbpTableList[0]);
                }

                var rates = nbpTableList[0].Rates;

                return rates;
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

        private static bool DataNotInRepository(List<NbpTable> nbpTableList, IEnumerable<NbpTable> repository)
        {
            return !repository.Any(r => r.EffectiveDate == nbpTableList[0].EffectiveDate);
        }
    }
}