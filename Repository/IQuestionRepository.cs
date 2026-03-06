using Assignment.DTOs;
using Assignment.Models;

namespace Assignment.Repository
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetQuestionsAsync();
        Task<Question> GetQuestionsByIdAsync(int id);
        Task<int> AddQuestionsAsync(Question photoId);
        Task UpdateQuestionsAsync(int id, Question photoId);
        Task DeleteQuestionsAsync(int id);
        Task<List<Question>> GetRandomQuestionsAsync(int number);
    }
}
