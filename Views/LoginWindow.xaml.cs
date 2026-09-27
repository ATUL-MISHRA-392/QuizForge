using System.Windows;
using QuizForge.Desktop.Services;

namespace QuizForge.Desktop.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AuthService _authService = new AuthService();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageTextBlock.Text = "Please enter your email.";
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageTextBlock.Text = "Please enter your password.";
                return;
            }

            try
            {
                LoginButton.IsEnabled = false;
                MessageTextBlock.Text = "Checking login...";

                var user = await _authService.LoginAsync(email, password);

                if (user == null)
                {
                    MessageBox.Show(
                        "Invalid email or password.",
                        "Login Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                MessageBox.Show(
                    $"Login successful!\nWelcome, {user.FullName}",
                    "Login Successful",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DashboardWindow dashboardWindow =
                    new DashboardWindow(user);

                dashboardWindow.Show();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageTextBlock.Text =
                    "Database connection error: " + ex.Message;
            }
            finally
            {
                LoginButton.IsEnabled = true;
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();

            registerWindow.Show();

            this.Close();
        }
    }
}