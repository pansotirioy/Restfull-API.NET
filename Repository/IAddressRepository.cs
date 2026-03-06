using Assignment.Models;

namespace Assignment.Repository
{
    public interface IAddressRepository
    {
        Task<int> AddAddressAsync(Address address);
        Task<Address> GetAddressByCandidateIdAsync(string id);
        Task DeleteAddressAsync(int id);
        Task<Address> GetAddressByIdAsync(int id);
        Task<List<Address>> GetAddressesAsync();
        Task UpdateAddressAsync(int id, Address address);
    }
}