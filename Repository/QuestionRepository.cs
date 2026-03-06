
using Assignment.Models;
using Assignment.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Assignment.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;
        public QuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Question>> GetQuestionsAsync()
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<Question> GetQuestionsByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id) + " must be greater than zero. (Thrown from GetQuestionsByIdAsync)");
            }
            Question? questions = await _context.Questions.FirstOrDefaultAsync(p => p.Id == id);
            if (questions == null)
            {
                throw new Exception("PhotoId not found (Thrown from GetQuestionsByIdAsync)");
            }
            return questions;
        }

        public async Task<int> AddQuestionsAsync(Question questions)
        {
            if (questions == null)
            {
                throw new ArgumentNullException(nameof(questions) + " Is Null (Thrown from AddQuestionsAsync)");
            }
            await _context.Questions.AddAsync(questions);
            await _context.SaveChangesAsync();
            return questions.Id;
        }

        public async Task UpdateQuestionsAsync(int id, Question questions)
        {
            if (questions == null)
            {
                throw new ArgumentNullException(nameof(questions) + "Is Null (Thrown from UpdateQuestionsAsync)");
            }
            else if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id) + " must be greater than zero. (Thrown from UpdateQuestionsAsync)");
            }
            Question existingQuestions = await GetQuestionsByIdAsync(id);

            existingQuestions.question = questions.question;
            existingQuestions.answer1 = questions.answer1;
            existingQuestions.answer2 = questions.answer2;
            existingQuestions.answer3 = questions.answer3;
            existingQuestions.answer4 = questions.answer4;
            existingQuestions.CandidatesAnalyticsId = questions.CandidatesAnalyticsId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionsAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentNullException(nameof(id) + " must be greater than zero. (Thrown from DeleteQuestionsAsync)");
            }
            Question questions = await GetQuestionsByIdAsync(id);
            _context.Questions.Remove(questions);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Question>> GetRandomQuestionsAsync(int number)
        {
            return await _context.Questions
                .OrderBy(q => Guid.NewGuid())
                .Take(number)
                .ToListAsync();
        }
    }
}
