using backend.Models;
using backend.Repositories;
using System.Text.Json;

public class CurrencyService
{
    private readonly ICurrencyRateRepository _repository;
    private readonly HttpClient _httpClient;

    public CurrencyService(ICurrencyRateRepository repository, IHttpClientFactory httpClientFactory)
    {
        _repository = repository;
        _httpClient = httpClientFactory.CreateClient("NBP");
    }

    public async Task<List<NbpRate>> GetTodayRatesAsync()
    {
        var existingRates = await _repository.GetAllAsync();
        var todayRates = existingRates
            .Where(e => e.effectiveDate.Equals(DateOnly.FromDateTime(DateTime.Now)))
            .SelectMany(e => e.rates)
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
            table = rate.table,
            no = rate.no,
            effectiveDate = rate.effectiveDate,
            rates = rate.rates.Select(r => new NbpRate
            {
                currency = r.currency,
                code = r.code,
                mid = r.mid
            }).ToList()
        };

        await _repository.AddAsync(nbpTable);
        return nbpTable.rates;
    }

    public async Task<List<NbpTable>> GetCurrencyRatesAsync()
    {
        var apiUrl = "https://api.nbp.pl/api/exchangerates/tables/A/?format=json";
        var response = await _httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Błąd podczas pobierania danych z API NBP.");
        }

        var content = await response.Content.ReadAsStringAsync();
        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };
        return JsonSerializer.Deserialize<List<NbpTable>>(content);
    }
}