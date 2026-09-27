using Microsoft.EntityFrameworkCore;
using QuizForge.Desktop.Data;

namespace QuizForge.Desktop.Services
{
    public class DashboardService
    {
        public async Task<int> GetTotalAttemptsAsync(int userId)
        {
            using var db = new AppDbContext();

            return await db.QuizAttempts
                .CountAsync(a => a.UserId == userId);
        }

        public async Task<int> GetTotalQuestionsAsync()
        {
            using var db = new AppDbContext();

            return await db.Questions.CountAsync();
        }

        public async Task<int> GetBestScoreAsync(int userId)
        {
            using var db = new AppDbContext();

            return await db.QuizAttempts
                .Where(a => a.UserId == userId)
                .Select(a => (int?)a.Score)
                .MaxAsync() ?? 0;
        }

        public async Task<double> GetAveragePercentageAsync(int userId)
        {
            using var db = new AppDbContext();

            var attempts = await db.QuizAttempts
                .Where(a =>
                    a.UserId == userId &&
                    a.TotalQuestions > 0)
                .Select(a =>
                    (double)a.Score / a.TotalQuestions * 100)
                .ToListAsync();

            if (attempts.Count == 0)
                return 0;

            return attempts.Average();
        }
    }
}