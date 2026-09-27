using System.Windows;
using QuizForge.Desktop.Services;

namespace QuizForge.Desktop.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly AuthService _authService = new AuthService();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text.Trim();
            string email = EmailTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(fullName))
            {
                MessageTextBlock.Text = "Please enter your full name.";
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageTextBlock.Text = "Please enter your email.";
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageTextBlock.Text = "Please enter a password.";
                return;
            }

            if (password.Length < 6)
            {
                MessageTextBlock.Text =
                    "Password must contain at least 6 characters.";
                return;
            }

            try
            {
                RegisterButton.IsEnabled = false;
                MessageTextBlock.Text = "Creating account...";

                bool success = await _authService.RegisterAsync(
                    fullName,
                    email,
                    password);

                if (!success)
                {
                    MessageTextBlock.Text =
                        "This email is already registered.";
                    return;
                }

                MessageTextBlock.Foreground =
                    new System.Windows.Media.SolidColorBrush(
                        System.Windows.Media.Colors.Green);

                MessageTextBlock.Text =
                    "Account created successfully!";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(),
                    "Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                RegisterButton.IsEnabled = true;
            }
        }
    }
}