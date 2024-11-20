using backend.Models;

namespace backend.Repositories
{
    public interface IRepository
    {
        Task AddAsync(NbpTable rate);
        Task<IEnumerable<NbpTable>> GetAllAsync();
        Task ClearAsync();
    }
}
