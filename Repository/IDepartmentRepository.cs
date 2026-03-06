using Assignment.Models;

namespace Assignment.Repository
{
    public interface IDepartmentRepository
    {
        Task<int> AddDepartmentAsync(Department department);
        Task DeleteDepartmentAsync(int id);
        Task<List<Department>> GetDepartmentAsync();
        Task<Department> GetDepartmentByIdAsync(int id);
        Task UpdateDepartmentAsync(int id, Department department);
    }
}