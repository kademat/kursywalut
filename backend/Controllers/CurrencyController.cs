using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrencyRates()
        {
            try
            {
                // Najpierw sprawdzamy, czy istnieją dzisiejsze kursy w repozytorium
                var todayRates = await _currencyService.GetTodayRatesFromRepositoryAsync();
                if (todayRates.Any())
                {
                    return Ok(todayRates);
                }

                // Jeśli nie, pobieramy je z API
                var ratesFromApi = await _currencyService.GetRatesFromApiAsync();
                if (ratesFromApi == null || !ratesFromApi.Any())
                {
                    return NotFound("Nie odnaleziono kursów w API.");
                }

                return Ok(ratesFromApi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}