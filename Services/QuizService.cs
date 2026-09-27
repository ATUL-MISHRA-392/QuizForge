using Microsoft.EntityFrameworkCore;
using QuizForge.Desktop.Data;
using QuizForge.Desktop.Models;

namespace QuizForge.Desktop.Services
{
    public class QuizService
    {
        public async Task SaveQuizAttemptAsync(
            int userId,
            int categoryId,
            int score,
            int totalQuestions,
            int correctAnswers,
            int wrongAnswers,
            int skippedAnswers,
            Dictionary<int, int?> selectedAnswers)
        {
            using var db = new AppDbContext();

            var attempt = new QuizAttempt
            {
                UserId = userId,
                CategoryId = categoryId,
                Score = score,
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers,
                WrongAnswers = wrongAnswers,
                SkippedAnswers = skippedAnswers,
                StartedAt = DateTime.Now,
                CompletedAt = DateTime.Now
            };

            db.QuizAttempts.Add(attempt);

            await db.SaveChangesAsync();

            foreach (var answer in selectedAnswers)
            {
                var question = await db.Questions
                    .Include(q => q.Options)
                    .FirstOrDefaultAsync(
                        q => q.QuestionId == answer.Key);

                if (question == null)
                    continue;

                int? selectedOptionId = answer.Value;

                bool isCorrect = false;

                if (selectedOptionId.HasValue)
                {
                    var selectedOption = question.Options
                        .FirstOrDefault(
                            o => o.OptionId == selectedOptionId.Value);

                    if (selectedOption != null)
                    {
                        isCorrect = selectedOption.IsCorrect;
                    }
                }

                var attemptAnswer = new AttemptAnswer
                {
                    AttemptId = attempt.AttemptId,
                    QuestionId = question.QuestionId,
                    SelectedOptionId = selectedOptionId,
                    IsCorrect = isCorrect,
                    TimeTaken = 0
                };

                db.AttemptAnswers.Add(attemptAnswer);
            }

            double percentage = totalQuestions > 0
                ? (double)score / totalQuestions * 100
                : 0;

            var history = new QuizHistory
            {
                UserId = userId,
                AttemptId = attempt.AttemptId,
                Score = score,
                Percentage = (decimal)percentage,
                CreatedAt = DateTime.Now
            };

            db.QuizHistory.Add(history);

            await db.SaveChangesAsync();
        }
    }
}