using Microsoft.EntityFrameworkCore;
using QuizForge.Desktop.Data;
using QuizForge.Desktop.Models;

namespace QuizForge.Desktop.Services
{
    public class QuestionService
    {
        public async Task<List<Question>> GetQuestionsByCategoryAsync(
            int categoryId)
        {
            using var db = new AppDbContext();

            return await db.Questions
                .Include(q => q.Options)
                .Where(q => q.CategoryId == categoryId)
                .OrderBy(q => q.QuestionId)
                .ToListAsync();
        }
    }
}