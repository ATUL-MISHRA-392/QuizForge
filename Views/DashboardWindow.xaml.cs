using System;
using System.Windows;
using QuizForge.Desktop.Models;
using QuizForge.Desktop.Services;

namespace QuizForge.Desktop.Views
{
    public partial class DashboardWindow : Window
    {
        private readonly User _loggedInUser;

        private readonly DashboardService _dashboardService =
            new DashboardService();

        public DashboardWindow(User user)
        {
            InitializeComponent();

            _loggedInUser = user;

            WelcomeTextBlock.Text =
                $"Welcome, {_loggedInUser.FullName}!";

            LoadDashboardStatistics();
        }

        private async void LoadDashboardStatistics()
        {
            try
            {
                int totalAttempts =
                    await _dashboardService.GetTotalAttemptsAsync(
                        _loggedInUser.UserId);

                int totalQuestions =
                    await _dashboardService.GetTotalQuestionsAsync();

                int bestScore =
                    await _dashboardService.GetBestScoreAsync(
                        _loggedInUser.UserId);

                double averagePercentage =
                    await _dashboardService.GetAveragePercentageAsync(
                        _loggedInUser.UserId);

                TotalAttemptsTextBlock.Text =
                    totalAttempts.ToString();

                TotalQuestionsTextBlock.Text =
                    totalQuestions.ToString();

                BestScoreTextBlock.Text =
                    bestScore.ToString();

                AveragePercentageTextBlock.Text =
                    $"{averagePercentage:0.##}%";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load dashboard statistics.\n\n" +
                    ex.Message,
                    "QuizForge",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LogoutButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            LoginWindow loginWindow =
                new LoginWindow();

            loginWindow.Show();

            this.Close();
        }

        private void StartQuizButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CategoriesWindow categoriesWindow =
                new CategoriesWindow(_loggedInUser);

            categoriesWindow.Show();

            this.Close();
        }

        private void CategoriesButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CategoriesWindow categoriesWindow =
                new CategoriesWindow(_loggedInUser);

            categoriesWindow.Show();

            this.Close();
        }

        private void HistoryButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            HistoryWindow historyWindow =
                new HistoryWindow(_loggedInUser);

            historyWindow.Show();

            this.Close();
        }
    }
}