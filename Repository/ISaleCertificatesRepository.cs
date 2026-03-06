using Assignment.Models;
using Assignment.DTOs;

namespace Assignment.Repository
{

    public interface ISaleCertificatesRepository
    {
        Task<List<SaleCertificate>> GetSaleCertificatesAsync();
        Task DeleteSaleCertificateAsync(int id);
        Task<SaleCertificate> GetSaleCertificateByIdAsync(int id);
        Task UpdateSaleCertificateAsync(int id, SaleCertificate SaleCertificate);
        Task<int> AddSaleCertificateAsync(SaleCertificate SaleCertificate);



    }
}
