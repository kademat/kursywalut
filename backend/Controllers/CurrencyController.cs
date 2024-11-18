using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public CurrencyController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("NBP");
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrencyRates()
        {
            // Endpoint API NBP
            var apiUrl = "https://api.nbp.pl/api/exchangerates/tables/A/?format=json";

            try
            {
                // Wysyłanie zapytania GET do API NBP
                var response = await _httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Błąd podczas pobierania danych z API NBP.");
                }

                // Odczyt odpowiedzi jako JSON
                var content = await response.Content.ReadAsStringAsync();
                try
                {
                    JsonSerializerOptions options = new()
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var rates = JsonSerializer.Deserialize<List<NbpTable>>(content);
                    return Ok(rates?[0]?.rates);// Pobieramy listę walut z pierwszej tabeli
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Błąd deserializacji: {ex.Message}");
                }
                return BadRequest();
                // Zwrot przetworzonych danych
                
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, $"Błąd podczas połączenia z API NBP: {ex.Message}");
            }
        }
    }

    // Klasy do mapowania odpowiedzi JSON z API NBP
    public class NbpTable
    {
        public string table { get; set; }
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public List<NbpRate> rates { get; set; }
    }

    public class NbpRate
    {
        public string currency { get; set; }
        public string code { get; set; }
        public decimal mid { get; set; }
    }
}