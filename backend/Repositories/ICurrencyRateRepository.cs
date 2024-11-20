using backend.Models;

namespace backend.Repositories
{
    public interface ICurrencyRateRepository
    {
        Task AddAsync(NbpTable rate);
        Task<IEnumerable<NbpTable>> GetAllAsync();
        Task ClearAsync();
    }
}
