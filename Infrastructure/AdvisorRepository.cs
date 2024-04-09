using AdvisorManagement.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvisorManagement.Infrastructure.Persistence
{
    public class AdvisorRepository : IAdvisorRepository
    {
        private readonly AppDbContext _context;
        private readonly MRUCache<int, string> _cache;

        public AdvisorRepository(AppDbContext context)
        {
            _context = context;
            MRUCache<int, string> cache = new MRUCache<int, string>(5);

        }

        public async Task AddAsync(Advisor advisor)
        {
            await _context.Advisors.AddAsync(advisor);
            await _context.SaveChangesAsync();
        }

        // Other CRUD operations can be implemented here
    }

    public interface IAdvisorRepository
    {
        Task AddAsync(Advisor advisor);
        // Other CRUD methods
    }
}