using Assignment.DTOs;
using Assignment.Models;
using Assignment.Service;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Assignment.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public AddressRepository(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task<List<Address>> GetAddressesAsync()
        {
            return await _context.Addresses.ToListAsync();
        }

        public async Task<Address> GetAddressByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id) + " must be greater than zero. (Thrown from GetAddressByIdAsync)");
            }
            Address? address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
            if (address == null)
            {
                throw new Exception("Address not found (Thrown from GetAddressByIdAsync)");
            }
            return address;
        }

        public async Task<Address> GetAddressByCandidateIdAsync(string id)
        {
            if (id.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(id) + " is Null. (Thrown from GetAddressByCandidateIdAsync)");
            }
            var output = _memoryCache.Get<Address>(id);
            if (output == null)
            {
                output = await _context.Addresses.FirstOrDefaultAsync(a => a.CandidateId == id);
                if (output == null)
                {
                    throw new Exception("Address not found (Thrown from GetAddressByCandidateIdAsync)");
                }
            }
            return output;
        }

        public async Task<int> AddAddressAsync(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address) + " Is Null (Thrown from AddAddressAsync)");
            }
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address.Id;
        }

        public async Task UpdateAddressAsync(int id, Address address)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id) + " must be greater than zero. (Thrown from UpdateAddressAsyncc)");
            }
            else if (address == null)
            {
                throw new ArgumentNullException(nameof(address) + "Is Null (Thrown from UpdateAddressAsync)");
            }
            Address existingAddress = await GetAddressByIdAsync(id);
            existingAddress.City = address.City;
            existingAddress.Street = address.Street;
            existingAddress.State = address.State;
            existingAddress.PostalCode = address.PostalCode;
            existingAddress.Country = address.Country;
            existingAddress.LandlineNumber = address.LandlineNumber;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAddressAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id) + " must be greater than zero. (Thrown from DeleteAddressAsync)");
            }
            Address address = await GetAddressByIdAsync(id);
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }
    }
}