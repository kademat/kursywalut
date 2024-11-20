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
        public async Task<IActionResult> GetCurrencyRates()
        {
            try
            {
                var rates = await _currencyService.GetCurrencyRatesAsync();

                if (rates == null || rates.Count == 0)
                {
                    return NotFound("Nie odnaleziono kursów walut.");
                }

                return Ok(rates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}