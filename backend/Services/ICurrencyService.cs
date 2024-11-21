using backend.Models;

namespace backend.Services
{
    public interface ICurrencyService
    {
        /// <summary>
        /// Pobiera wszystkie kursy walut dla danego dnia.
        /// </summary>
        /// <returns>Lista tabel kursów walut.</returns>
        Task<IList<NbpRate>> GetCurrencyRatesAsync();
    }
}