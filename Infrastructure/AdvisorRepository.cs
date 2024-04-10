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

        public async Task UpdateAsync(Advisor advisor)
        {
            _context.Advisors.Update(advisor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Advisor advisor)
        {
            _context.Advisors.Remove(advisor);
            await _context.SaveChangesAsync();
        }

        public Advisor GetAsync(int advisorId)
        {
            return _context.Advisors.FirstOrDefault(x => x.Id == advisorId);
        }
    }

    public interface IAdvisorRepository
    {
        Task AddAsync(Advisor advisor);
        Task UpdateAsync(Advisor advisor);
        Task DeleteAsync(Advisor advisor);
        Advisor GetAsync(int advisorId);        
    }
}