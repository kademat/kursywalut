using backend.Models;

namespace backend.Services
{
    public interface ICurrencyService
    {
        /// <summary>
        /// Pobiera aktualne kursy walut z tabeli NBP (zdefinowanej w CurrencyTable).
        /// </summary>
        /// <param name="currencyTable">Określa, czy mają być pobrane kursy z tabeli A (główne waluty, MainCurrencyTable) czy tabeli B (rzadsze waluty, MinorCurrencyTable).</param>
        /// <returns>Lista kursów walut, reprezentowana przez obiekty typu <see cref="NbpRate"/>.</returns>
        Task<IList<NbpRate>> GetCurrencyRatesAsync(CurrencyTable currencyTable);
    }
}