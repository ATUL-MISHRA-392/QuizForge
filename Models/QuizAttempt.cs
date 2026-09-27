namespace QuizForge.Desktop.Models
{
    public class QuizAttempt
    {
        public int AttemptId { get; set; }

        public int UserId { get; set; }

        public int CategoryId { get; set; }

        public int Score { get; set; }

        public int TotalQuestions { get; set; }

        public int CorrectAnswers { get; set; }

        public int WrongAnswers { get; set; }

        public int SkippedAnswers { get; set; }

        public DateTime StartedAt { get; set; } = DateTime.Now;

        public DateTime? CompletedAt { get; set; }

        public User? User { get; set; }

        public Category? Category { get; set; }

        public List<AttemptAnswer> Answers { get; set; } = new();
    }
}