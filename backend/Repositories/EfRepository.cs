using Microsoft.EntityFrameworkCore;
using backend.Data;

namespace backend.Repositories
{
    public class EfRepository : IRepository
    {
        private readonly AppDbContext _context;

        public EfRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NbpTable table)
        {
            // Dodanie nowego rekordu
            await _context.NbpTables.AddAsync(table);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NbpTable>> GetAllAsync()
        {
            return await _context.NbpTables
                .Include(t => t.Rates) // Kursy walut powiązane z tabelą
                .ToListAsync();
        }

        // Czyszczenie tabeli kursów walut (usunięcie wszystkich rekordów)
        public async Task ClearAsync()
        {
            _context.NbpTables.RemoveRange(_context.NbpTables);
            await _context.SaveChangesAsync();
        }
    }
}