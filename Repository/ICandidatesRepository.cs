
using Assignment.Models;
using Assignment.DTOs;

namespace Assignment.Repository
{
    public interface ICandidatesRepository
    {
        Task DeleteCandidateAsync(string id);
        Task<Candidate> GetCandidateByIdAsync(string id);
        Task<Candidate> GetCandidateByUserNameAsync(string username);
        Task<List<Candidate>> GetCandidatesAsync();
        Task UpdateCandidateAsync(string id, Candidate candidate);
    }
}
