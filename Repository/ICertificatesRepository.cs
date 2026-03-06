using Assignment.Models;

namespace Assignment.Repository
{
    public interface ICertificatesRepository
    {
        Task<int> AddCertificateAsync(Certificate certificate);
        Task AddCertificateCandidateAnalitycsAsync(int CId, int CAId);
        Task AddCertificatesFromSaleAsync(string candidateId, int saleCertificateId);
        Task DeleteCertificateAsync(int id);
        Task<List<Certificate>> GetAllCertificatesByCandidateIdAsync(string CandidateId);
        Task<Certificate> GetCertificateByIdAsync(int id);
        Task<IEnumerable<object>> MarksPerTopicPerCertificateAsync(string candidateId);
        Task<List<SaleCertificate>> NotObtainedCertificatesByCandidateIdAsync(string candidateId);
        Task RemoveCertificateCandidateAnalitycsAsync(int CId, int CAId);
        Task UpdateCertificateAsync(int id, Certificate certificate);
    }
}