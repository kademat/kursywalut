using backend.Models;
using backend.Repositories;
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
                // Najpierw sprawdzamy, czy istnieją dzisiejsze kursy
                var todayRates = await _currencyService.GetTodayRatesAsync();
                if (todayRates.Any())
                {
                    return Ok(todayRates);
                }

                // Jeśli nie, pobieramy je z API
                var ratesFromApi = await _currencyService.GetRatesFromApiAsync();
                if (ratesFromApi == null || !ratesFromApi.Any())
                {
                    return NotFound("No currency rates found.");
                }

                return Ok(ratesFromApi);


                //var existingRates = await _repository.GetAllAsync();

                //// Filtruj tylko dane z dzisiejszym effectiveDate
                //var todayRates = existingRates
                //    .Where(e => e.effectiveDate.Equals(DateOnly.FromDateTime(DateTime.Now)))
                //    .ToList();

                //if (todayRates.Any())
                //{
                //    // Zwróć tylko dane z dzisiejszego dnia
                //    return Ok(todayRates.Select(r => r.rates).ToList());
                //}

                //var rates = await _currencyService.GetCurrencyRatesAsync();

                //if (rates == null || !rates.Any())
                //{
                //    return NotFound("No currency rates found.");
                //}
                //if(!existingRates.Any(er => er.effectiveDate == rates[0].effectiveDate))
                //{
                //    var nbpTable = new NbpTable
                //    {
                //        table = rates[0].table,
                //        no = rates[0].no,
                //        effectiveDate = rates[0].effectiveDate,
                //        rates = [] // Inicjalizujemy listę NbpRate
                //    };

                //    foreach (var rate in rates[0].rates)
                //    {
                //        var nbpRate = new NbpRate
                //        {
                //            currency = rate.currency,
                //            code = rate.code,
                //            mid = rate.mid
                //        };

                //        // Dodajemy kurs waluty do listy w obiekcie NbpTable
                //        nbpTable.rates.Add(nbpRate);
                //    }
                //    await _repository.AddAsync(nbpTable);
                //}

                //return Ok(rates?[0].rates); // Pobieramy listę walut z pierwszej tabeli
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}