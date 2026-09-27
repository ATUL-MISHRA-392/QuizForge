namespace QuizForge.Desktop.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        public int CategoryId { get; set; }

        public string QuestionText { get; set; } = string.Empty;

        public string? ImagePath { get; set; }

        public string Difficulty { get; set; } = "Medium";

        public int TimeLimit { get; set; } = 30;

        public Category? Category { get; set; }

        public List<QuizOption> Options { get; set; } = new();
    }
}