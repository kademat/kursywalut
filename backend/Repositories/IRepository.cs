using backend.Models;

namespace backend.Repositories
{
    public interface IRepository
    {
        Task AddAsync(NbpTable table);
        Task<IEnumerable<NbpTable>> GetAllAsync();
        Task ClearAsync();
    }
}
