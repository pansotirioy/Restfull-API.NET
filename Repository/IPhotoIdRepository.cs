using Assignment.DTOs;
using Assignment.Models;

namespace Assignment.Repository
{
    public interface IPhotoIdRepository
    {
        Task<List<PhotoId>> GetPhotoIdsAsync();
        Task<PhotoId> GetPhotoIdByIdAsync(int id);
        Task<int> AddPhotoIdAsync(PhotoId photoId);
        Task UpdatePhotoIdAsync(int id,PhotoId photoId);
        Task DeletePhotoIdAsync(int id);
    }
}
