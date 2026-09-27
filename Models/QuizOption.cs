namespace QuizForge.Desktop.Models
{
    public class QuizOption
    {
        public int OptionId { get; set; }

        public int QuestionId { get; set; }

        public string OptionText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }

        public Question? Question { get; set; }
    }
}