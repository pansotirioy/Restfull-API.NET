using Assignment.Models;
using Assignment.DTOs;

namespace Assignment.Repository
{

    public interface ICertificateRepository
    {
        Task<List<Certificate>> GetCertificatesAsync(string CandidateId);
        Task DeleteCertificateAsync(int id);
        Task<Certificate> GetCertificateByIdAsync(int id);
        Task UpdateCertificateAsync(int id, Certificate certificate);
        Task<int> AddCertificateAsync(Certificate certificate);
        Task AddCertificateCandidateAnalitycsAsync(int CId, int CAId);
        Task RemoveCertificateCandidateAnalitycsAsync(int CId, int CAId);



    }
}
