using AdvisorManagement.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvisorManagement.Infrastructure.Persistence
{
    public class AdvisorRepository : IAdvisorRepository
    {
        private readonly AppDbContext _context;
        private readonly MRUCache<int, Advisor> _cache;

        public AdvisorRepository(AppDbContext context, MRUCache<int, Advisor> cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task AddAsync(Advisor advisor)
        {
            advisor.HealthStatus = AdvisorRepository.GetColor();
            await _context.Advisors.AddAsync(advisor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Advisor advisor)
        {
            _context.Advisors.Update(advisor);
            await _context.SaveChangesAsync();
            _cache.Put(advisor.Id, advisor); // Update cache
        }

        public async Task DeleteAsync(int advisorId)
        {
            var advisor = await _context.Advisors.FindAsync(advisorId);
            if (advisor != null)
            {
                _context.Advisors.Remove(advisor);
                await _context.SaveChangesAsync();
                _cache.Delete(advisorId); // Remove from cache
            };
        }

        public async Task<Advisor> GetAsync(int advisorId)
        {
            // Check if advisor exists in cache
            var advisor = _cache.TryGet(advisorId);
            
            if (advisor != null)
                return advisor;

            // If not found in cache, retrieve from database and add to cache
            advisor = await _context.Advisors.FindAsync(advisorId);
            if (advisor != null)
                _cache.Put(advisorId, advisor);

            return advisor;
        }

        public List<Advisor> GetAll()
        {
            return _context.Advisors.ToList();         
        }

        public static string GetColor()
        {
            var hColors = new Dictionary<int, string>() { { 60, "Green" }, { 80, "Yellow" }, { 100, "Green" } };

            var random = new Random();
            int rndprob = random.Next(100);

            var retColor = hColors.Last(x => rndprob <= x.Key);

            return retColor.Value;
        }
    }

    public interface IAdvisorRepository
    {
        Task AddAsync(Advisor advisor);
        Task UpdateAsync(Advisor advisor);
        Task DeleteAsync(int advisorId);
        Task<Advisor> GetAsync(int advisorId);
        List<Advisor> GetAll();

    }
}