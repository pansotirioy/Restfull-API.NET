using Assignment.Models;
using Assignment.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using NuGet.ContentModel;
using System.Globalization;

namespace Assignment.Repository
{
    public class CandidatesRepository : ICandidatesRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private const string CandidatesCacheKey = "CandidatesCacheKey";

        public CandidatesRepository(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<List<Candidate>> GetCandidatesAsync()
        {
            var output = _memoryCache.Get<List<Candidate>>(CandidatesCacheKey);
            if (output == null)
            {
                output = await _context.Candidates.ToListAsync();
                _memoryCache.Set(CandidatesCacheKey, output, TimeSpan.FromMinutes(5));
            }
            return output;
        }

        public async Task<Candidate> GetCandidateByIdAsync(string id)
        {
            if (id.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(id) + "ID must be greater than zero. (Thrown from GetCandidateByIdAsync)");
            }
            Candidate? candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Id == id);
            if (candidate == null)
            {
                throw new Exception("Candidate not found (Thrown from GetCandidateByIdAsync)");
            }
            return candidate;
        }

        public async Task<Candidate> GetCandidateByUserNameAsync(string username)
        {
            if (username.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(username) + " is Null. (Thrown from GetCandidateByUserNameAsync)");
            }
            var candidate = _memoryCache.Get<Candidate>(CandidatesCacheKey + "_" + username);
            if (candidate == null)
            {
                candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.UserName == username);
                if (candidate == null)
                {
                    throw new Exception("Candidate not found (Thrown from GetCandidateByUserNameAsync)");
                }
                _memoryCache.Set(CandidatesCacheKey + "_" + username, candidate, TimeSpan.FromMinutes(5));
            }
            return candidate;
        }

        public async Task UpdateCandidateAsync(string id, Candidate candidate)
        {
            if (candidate == null)
            {
                throw new ArgumentNullException(nameof(candidate) + "Is Null (Thrown from UpdateCandidateAsync)");
            }
            else if (id.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(id) + "ID must be greater than zero. (Thrown from UpdateCandidateAsync)");
            }
            Candidate existingCandidate = await GetCandidateByIdAsync(id);
            existingCandidate.FirstName = candidate.FirstName;
            existingCandidate.LastName = candidate.LastName;
            existingCandidate.MiddleName = candidate.MiddleName;
            existingCandidate.Gender = candidate.Gender;
            existingCandidate.DateOfBirth = candidate.DateOfBirth;
            existingCandidate.Email = candidate.Email;
            existingCandidate.NativeLanguage = candidate.NativeLanguage;
            existingCandidate.PhoneNumber = candidate.PhoneNumber;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCandidateAsync(string id)
        {
            if (id.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(id) + " must be greater than zero. (Thrown from DeleteCandidateAsync)");
            }
            Candidate existingCandidate = await GetCandidateByIdAsync(id);
            _context.Candidates.Remove(existingCandidate);

            await _context.SaveChangesAsync();
        }

       
    }
}