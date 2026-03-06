using Assignment.DTOs;

namespace Assignment.Service
{
    public interface IAiRoutingService
    {
        Task<string> DetectDepartment(string message,int converastion);
    }
}
