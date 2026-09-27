namespace QuizForge.Desktop.Models
{
    public class QuizHistory
    {
        public int HistoryId { get; set; }

        public int UserId { get; set; }

        public int AttemptId { get; set; }

        public int Score { get; set; }

        public decimal Percentage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User? User { get; set; }

        public QuizAttempt? Attempt { get; set; }
    }
}