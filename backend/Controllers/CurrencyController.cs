using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        // Na ten moment symulowane kursów walut
        [HttpGet]
        public IActionResult GetCurrencyRates()
        {
            var rates = new List<object>
            {
                new { Currency = "USD", Mid = 4.5 },
                new { Currency = "EUR", Mid = 4.2 },
                new { Currency = "GBP", Mid = 5.3 }
            };

            return Ok(rates);
        }
    }
}