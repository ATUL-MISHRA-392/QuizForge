using Microsoft.EntityFrameworkCore;
using QuizForge.Desktop.Data;
using QuizForge.Desktop.Models;

namespace QuizForge.Desktop.Services
{
    public class HistoryService
    {
        public async Task<List<QuizHistory>> GetHistoryAsync(int userId)
        {
            using var db = new AppDbContext();

            return await db.QuizHistory
                .Include(h => h.Attempt)
                .ThenInclude(a => a!.Category)
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }
    }
}