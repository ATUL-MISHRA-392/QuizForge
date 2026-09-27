namespace QuizForge.Desktop.Models
{
    public class AttemptAnswer
    {
        public int AttemptAnswerId { get; set; }

        public int AttemptId { get; set; }

        public int QuestionId { get; set; }

        public int? SelectedOptionId { get; set; }

        public bool IsCorrect { get; set; }

        public int TimeTaken { get; set; }

        public QuizAttempt? Attempt { get; set; }

        public Question? Question { get; set; }

        public QuizOption? SelectedOption { get; set; }
    }
}