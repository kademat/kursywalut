using backend.Models;

namespace backend.Services
{
    public interface ICurrencyService
    {
        /// <summary>
        /// Pobiera wszystkie kursy walut.
        /// </summary>
        /// <returns>Lista tabel kursów walut.</returns>
        Task<List<NbpTable>> GetCurrencyRatesAsync();
    }
}