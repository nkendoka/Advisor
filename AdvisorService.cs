using AdvisorManagement.Model;
using AdvisorManagement.Infrastructure.Persistence;
using System.Threading.Tasks;
using System;

namespace AdvisorManager.Services
{
    public class AdvisorService
    {
        private readonly IAdvisorRepository _repository;

        public AdvisorService(IAdvisorRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAdvisorAsync(Advisor advisor)
        {
            await _repository.AddAsync(advisor);
        }

        public async Task UpdateAdvisorAsync(Advisor advisor)
        {
            await _repository.UpdateAsync(advisor);
        }
        
        public async Task DeleteAdvisorAsync(Advisor advisor)
        {
            await _repository.DeleteAsync(advisor);
        }

        public Advisor GetAdvisor(int advisorId)
        {
            return _repository.GetAsync(advisorId);
        }

        public static string GetColor()
        {
            var hColors = new Dictionary<int, string>() { { 60, "Green" }, { 80, "Yellow" }, { 100, "Green" } };
            
            var  random = new Random();
            int rndprob = random.Next(100);

            var retColor = hColors.Last(x=> rndprob <= x.Key);

            return retColor.Value;
        }
    }
}
