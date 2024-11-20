
namespace backend.Repositories
{
    public class InMemoryRepository : IRepository
    {
        private readonly List<NbpTable> _currencyRates = [];

        public Task AddAsync(NbpTable rate)
        {
            _currencyRates.Add(rate);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<NbpTable>> GetAllAsync()
        {
            return Task.FromResult(_currencyRates.AsEnumerable());
        }

        public Task ClearAsync()
        {
            _currencyRates.Clear();
            return Task.CompletedTask;
        }
    }
}