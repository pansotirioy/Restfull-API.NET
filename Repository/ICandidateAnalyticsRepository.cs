using Assignment.Models;

namespace Assignment.Repository
{
    public interface ICandidatesAnalyticsRepository
    {
        Task<List<CandidatesAnalytics>> GetCandidatesAnalyticsAsync();
        Task<CandidatesAnalytics> GetCandidatesAnalyticsByIdAsync(int id);
        Task<int> AddCandidatesAnalyticsAsync(CandidatesAnalytics analytics);
        Task UpdateCandidatesAnalyticsAsync(int id, CandidatesAnalytics analytics);
        Task DeleteCandidatesAnalyticsAsync(int id);
    }
}

