using System.Windows;
using QuizForge.Desktop.Models;

namespace QuizForge.Desktop.Views
{
    public partial class ResultWindow : Window
    {
        private readonly User _loggedInUser;

        public ResultWindow(
            User user,
            string categoryName,
            int score,
            int totalQuestions,
            int correctAnswers,
            int wrongAnswers,
            int skippedAnswers)
        {
            InitializeComponent();

            _loggedInUser = user;

            CategoryTextBlock.Text = categoryName;

            ScoreTextBlock.Text =
                $"{score} / {totalQuestions}";

            CorrectTextBlock.Text =
                correctAnswers.ToString();

            WrongTextBlock.Text =
                wrongAnswers.ToString();

            SkippedTextBlock.Text =
                skippedAnswers.ToString();

            double percentage =
                totalQuestions > 0
                    ? (double)score / totalQuestions * 100
                    : 0;

            PercentageTextBlock.Text =
                $"Percentage: {percentage:0.##}%";
        }

        private void DashboardButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            DashboardWindow dashboardWindow =
                new DashboardWindow(_loggedInUser);

            dashboardWindow.Show();

            this.Close();
        }
    }
}