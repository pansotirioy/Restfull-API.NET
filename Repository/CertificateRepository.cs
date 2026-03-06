
using Assignment.Models;
using Assignment.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace Assignment.Repository
{
    public class CertificatesRepository : ICertificatesRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public CertificatesRepository(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<List<Certificate>> GetAllCertificatesByCandidateIdAsync(string CandidateId)
        {
            var output = _memoryCache.Get<List<Certificate>>(CandidateId);
            if (output == null)
            {
                output = await _context.Certificates.Where(c => c.CandidateId == CandidateId).ToListAsync();
                _memoryCache.Set(CandidateId, output, TimeSpan.FromMinutes(1)); // Cache for 1 minutes because Certificates can change more frequently than SaleCertificates, especially when candidates are taking exams and receiving new certificates.
            }
            return output;
        }

        public async Task<Certificate> GetCertificateByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id) + " must be greater than zero. (Thrown from GetCertificateByIdAsync)");
            }

            Certificate? certificate = await _context.Certificates
                .FirstOrDefaultAsync(c => c.Id == id);

            if (certificate == null)
            {
                throw new Exception("Certificate not found (Thrown from GetCertificateByIdAsync)");
            }

            return certificate;
        }

        public async Task<int> AddCertificateAsync(Certificate certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate) + " is null (Thrown from AddCertificateAsync)");
            }

            await _context.Certificates.AddAsync(certificate);
            await _context.SaveChangesAsync();
            return certificate.Id;
        }

        public async Task UpdateCertificateAsync(int id, Certificate certificate)
        {
            if (certificate == null)
            {
                throw new ArgumentNullException(nameof(certificate) + " is null (Thrown from UpdateCertificateAsync)");
            }

            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id) + " must be greater than zero. (Thrown from UpdateCertificateAsync)");
            }

            Certificate existingCertificate = await GetCertificateByIdAsync(id);

            // Update fields
            existingCertificate.Title = certificate.Title;
            existingCertificate.AssessmentTestCode = certificate.AssessmentTestCode;
            existingCertificate.Description = certificate.Description;
            existingCertificate.ExaminationDate = certificate.ExaminationDate;
            existingCertificate.ScoreReportDate = certificate.ScoreReportDate;
            existingCertificate.CandidateScore = certificate.CandidateScore;
            existingCertificate.MaximumScore = certificate.MaximumScore;
            existingCertificate.PercentageScore = certificate.PercentageScore;
            existingCertificate.Price = certificate.Price;
            existingCertificate.AssessmentResultLabel = certificate.AssessmentResultLabel;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCertificateAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id) + " must be greater than zero. (Thrown from DeleteCertificateAsync)");
            }

            Certificate existingCertificate = await GetCertificateByIdAsync(id);

            _context.Certificates.Remove(existingCertificate);
            await _context.SaveChangesAsync();
        }

        public async Task AddCertificateCandidateAnalitycsAsync(int CId, int CAId)
        {
            if (CId <= 0 || CAId <= 0)
            {
                throw new ArgumentNullException("Id's must be greater than zero. (addCertificateCandidateAnalitycsAsync)");
            }

            Certificate? existingCertificate = await _context.Certificates.Include(e => e.CandidatesAnalytics).FirstOrDefaultAsync(e => e.Id == CId);

            if (existingCertificate == null)
                throw new InvalidOperationException("Certificate not found.");

            // Initialize collection if null
            existingCertificate.CandidatesAnalytics ??= new List<CandidatesAnalytics>();

            CandidatesAnalytics? candidatesAnalytics = await _context.CandidatesAnalytics.FirstOrDefaultAsync(e => e.Id == CAId);

            if (candidatesAnalytics == null)
                throw new InvalidOperationException("Candidates Analytics not found.");

            existingCertificate.CandidatesAnalytics.Add(candidatesAnalytics);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCertificateCandidateAnalitycsAsync(int CId, int CAId)
        {
            if (CId <= 0 || CAId <= 0)
            {
                throw new ArgumentNullException("Id's must be greater than zero. (addCertificateCandidateAnalitycsAsync)");
            }

            Certificate? existingCertificate = await _context.Certificates.Include(e => e.CandidatesAnalytics).FirstOrDefaultAsync(e => e.Id == CId);

            if (existingCertificate == null)
                throw new InvalidOperationException("Certificate not found.");

            // Initialize collection if null
            existingCertificate.CandidatesAnalytics ??= new List<CandidatesAnalytics>();

            CandidatesAnalytics? candidatesAnalytics = await _context.CandidatesAnalytics.FirstOrDefaultAsync(e => e.Id == CAId);

            if (candidatesAnalytics == null)
                throw new InvalidOperationException("Candidates Analytics not found.");

            existingCertificate.CandidatesAnalytics.Remove(candidatesAnalytics);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Object>> MarksPerTopicPerCertificateAsync(string candidateId)
        {
            if (candidateId.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(candidateId) + "Is Null (Thrown from MarksPerTopicPerCertificateAsync)");
            }
            var certificates = await _context.Certificates
                .Where(c => c.CandidateId == candidateId)
                .Include(cert => cert.CandidatesAnalytics)
                .ToListAsync();

            if (certificates == null! || !certificates.Any())
            {
                throw new KeyNotFoundException("No certificates found for the candidate (Thrown from MarksPerTopicPerCertificateAsync)");
            }

            var marksPerTopicWithTitle = certificates.Select(c => new
            {
                CertificateId = c.Id,
                CertificateTitle = c.Title,
                Analytics = c.CandidatesAnalytics?.ToDictionary(
                    ca => ca.TopicDescription ?? "Unknown Topic",
                    ca => ca.AwardedMarks
                )
            }).ToList();

            if (marksPerTopicWithTitle == null! || !marksPerTopicWithTitle.Any())
            {
                throw new KeyNotFoundException("No analytics found for the candidate's certificates (Thrown from MarksPerTopicPerCertificateAsync)");
            }

            return marksPerTopicWithTitle;
        }

        public async Task<List<SaleCertificate>> NotObtainedCertificatesByCandidateIdAsync(string candidateId)
        {
            if (candidateId.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(candidateId), " is Null. You must give an CandidateId.");
            }
            var obtainedCertificates = await GetAllCertificatesByCandidateIdAsync(candidateId);

            var allCertificates = await _context.SaleCertificates.ToListAsync();
            if (allCertificates == null)
            {
                throw new KeyNotFoundException(nameof(allCertificates) + "Is Null (Thrown from NotObtainedCertificatesByCandidateIdAsync)");
            }

            var obtainedTitles = new HashSet<string?>(obtainedCertificates.Select(c => c.Title));

            var notObtainedCertificates = allCertificates
                .Where(c => !(obtainedTitles.Contains(c.Title)))
                .ToList() ?? new List<SaleCertificate>();

            return notObtainedCertificates;
        }

        public async Task AddCertificatesFromSaleAsync(string candidateId, int saleCertificateId)
        {
            if (saleCertificateId == 0 || candidateId.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(saleCertificateId) + " or " + nameof(candidateId) + " must be a valid Id.");
            }

            var saleCertificate = await _context.SaleCertificates.FirstOrDefaultAsync(c => c.Id == saleCertificateId);

            var certificate = new Certificate
            {
                Title = saleCertificate!.Title,
                AssessmentTestCode = saleCertificate.AssessmentTestCode,
                Description = saleCertificate.Description,
                ExaminationDate = DateOnly.FromDateTime(DateTime.Now).AddDays(10),
                CandidateId = candidateId
            };

            await AddCertificateAsync(certificate);
        }
    }
}
