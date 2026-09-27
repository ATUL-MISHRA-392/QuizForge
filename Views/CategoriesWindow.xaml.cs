using System.Windows;
using QuizForge.Desktop.Models;
using QuizForge.Desktop.Services;

namespace QuizForge.Desktop.Views
{
    public partial class CategoriesWindow : Window
    {
        private readonly CategoryService _categoryService =
            new CategoryService();

        private readonly User _loggedInUser;

        public CategoriesWindow(User user)
        {
            InitializeComponent();

            _loggedInUser = user;

            LoadCategories();
        }

        private async void LoadCategories()
        {
            try
            {
                MessageTextBlock.Text = "Loading categories...";

                var categories =
                    await _categoryService.GetCategoriesAsync();

                CategoriesItemsControl.ItemsSource = categories;

                MessageTextBlock.Text = "";

                if (categories.Count == 0)
                {
                    MessageTextBlock.Text =
                        "No categories found.";
                }
            }
            catch (Exception ex)
            {
                MessageTextBlock.Text =
                    "Database error: " + ex.Message;
            }
        }

        private void StartQuizButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button button &&
                button.DataContext is Category category)
            {
                QuizWindow quizWindow =
                    new QuizWindow(
                        _loggedInUser,
                        category.CategoryId,
                        category.CategoryName);

                quizWindow.Show();

                this.Close();
            }
        }
    }
}