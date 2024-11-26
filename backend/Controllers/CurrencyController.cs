using backend.Mappers;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMainCurrencyRates()
        {
            try
            {
                var rates = await _currencyService.GetCurrencyRatesAsync(CurrencyTable.MainCurrencyTable);

                if (rates == null || rates.Count == 0)
                {
                    return NotFound("Nie odnaleziono głównych kursów walut.");
                }
                // Tutaj można by użyć automappera
                var ratesDto = rates.Select(r => r.ToDto()).ToList();
                return Ok(ratesDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("minor")]
        public async Task<IActionResult> GetMinorCurrencyRates()
        {
            try
            {
                var rates = await _currencyService.GetCurrencyRatesAsync(CurrencyTable.MinorCurrencyTable);

                if (rates == null || rates.Count == 0)
                {
                    return NotFound("Nie odnaleziono głównych kursów walut.");
                }

                // Tutaj można by użyć automappera
                var ratesDto = rates.Select(r => r.ToDto()).ToList();
                return Ok(ratesDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}