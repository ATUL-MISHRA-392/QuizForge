using System.Windows;
using QuizForge.Desktop.Models;
using QuizForge.Desktop.Services;

namespace QuizForge.Desktop.Views
{
    public partial class HistoryWindow : Window
    {
        private readonly HistoryService _historyService =
            new HistoryService();

        private readonly User _loggedInUser;

        public HistoryWindow(User user)
        {
            InitializeComponent();

            _loggedInUser = user;

            WelcomeTextBlock.Text =
                $"Quiz attempts for {_loggedInUser.FullName}";

            LoadHistory();
        }

        private async void LoadHistory()
        {
            try
            {
                MessageTextBlock.Text = "Loading quiz history...";

                var history =
                    await _historyService.GetHistoryAsync(
                        _loggedInUser.UserId);

                HistoryDataGrid.ItemsSource = history;

                if (history.Count == 0)
                {
                    MessageTextBlock.Text =
                        "No quiz attempts found.";
                }
                else
                {
                    MessageTextBlock.Text =
                        $"Total attempts: {history.Count}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load quiz history.\n\n" +
                    ex.Message,
                    "QuizForge",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
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