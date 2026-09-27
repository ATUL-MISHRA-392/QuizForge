using System.Windows;
using System.Windows.Threading;
using QuizForge.Desktop.Models;
using QuizForge.Desktop.Services;

namespace QuizForge.Desktop.Views
{
    public partial class QuizWindow : Window
    {
        private readonly QuestionService _questionService =
            new QuestionService();

        private readonly QuizService _quizService =
            new QuizService();

        private readonly User _loggedInUser;
        private readonly int _categoryId;
        private readonly string _categoryName;

        private List<Question> _questions = new();
        private int _currentQuestionIndex = 0;
        private readonly Dictionary<int, int?> _selectedAnswers = new();

        private DispatcherTimer _timer = new();
        private int _timeLeft;

        public QuizWindow(
            User user,
            int categoryId,
            string categoryName)
        {
            InitializeComponent();

            _loggedInUser = user;
            _categoryId = categoryId;
            _categoryName = categoryName;

            CategoryTextBlock.Text = categoryName;

            LoadQuestions();
        }

        private async void LoadQuestions()
        {
            try
            {
                _questions =
                    await _questionService.GetQuestionsByCategoryAsync(
                        _categoryId);

                if (_questions.Count == 0)
                {
                    MessageBox.Show(
                        "No questions found for this category.",
                        "QuizForge",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    Close();
                    return;
                }

                QuizProgressBar.Maximum = _questions.Count;

                ShowQuestion();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Database error:\n" + ex.Message,
                    "QuizForge",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Close();
            }
        }

        private void ShowQuestion()
        {
            if (_questions.Count == 0)
                return;

            Question question = _questions[_currentQuestionIndex];

            // Keep options in a fixed order
            question.Options = question.Options
                .OrderBy(o => o.OptionId)
                .ToList();

            QuestionNumberTextBlock.Text =
                $"Question {_currentQuestionIndex + 1} of {_questions.Count}";

            QuestionTextBlock.Text =
                question.QuestionText;

            QuizProgressBar.Value =
                _currentQuestionIndex + 1;

            OptionA.Content =
                question.Options.Count > 0
                    ? question.Options[0].OptionText
                    : "";

            OptionB.Content =
                question.Options.Count > 1
                    ? question.Options[1].OptionText
                    : "";

            OptionC.Content =
                question.Options.Count > 2
                    ? question.Options[2].OptionText
                    : "";

            OptionD.Content =
                question.Options.Count > 3
                    ? question.Options[3].OptionText
                    : "";

            // Reset radio buttons
            OptionA.IsChecked = false;
            OptionB.IsChecked = false;
            OptionC.IsChecked = false;
            OptionD.IsChecked = false;

            // Restore previously selected answer
            if (_selectedAnswers.TryGetValue(
                    question.QuestionId,
                    out int? selectedOptionId))
            {
                if (selectedOptionId == question.Options.ElementAtOrDefault(0)?.OptionId)
                    OptionA.IsChecked = true;

                else if (selectedOptionId == question.Options.ElementAtOrDefault(1)?.OptionId)
                    OptionB.IsChecked = true;

                else if (selectedOptionId == question.Options.ElementAtOrDefault(2)?.OptionId)
                    OptionC.IsChecked = true;

                else if (selectedOptionId == question.Options.ElementAtOrDefault(3)?.OptionId)
                    OptionD.IsChecked = true;
            }

            PreviousButton.IsEnabled =
                _currentQuestionIndex > 0;

            NextButton.Content =
                _currentQuestionIndex == _questions.Count - 1
                    ? "Finish"
                    : "Next →";

            StartTimer(question.TimeLimit);
        }

        private void StartTimer(int seconds)
        {
            _timer.Stop();

            _timeLeft = seconds;

            TimerTextBlock.Text =
                _timeLeft.ToString();

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timer.Tick += Timer_Tick;

            _timer.Start();
        }

        private void Timer_Tick(
            object? sender,
            EventArgs e)
        {
            _timeLeft--;

            TimerTextBlock.Text =
                _timeLeft.ToString();

            if (_timeLeft <= 0)
            {
                _timer.Stop();

                MoveToNextQuestion();
            }
        }

        private void SaveCurrentAnswer()
        {
            if (_questions.Count == 0)
                return;

            Question question = _questions[_currentQuestionIndex];

            int? selectedOptionId = null;

            if (OptionA.IsChecked == true &&
                question.Options.Count > 0)
            {
                selectedOptionId = question.Options[0].OptionId;
            }
            else if (OptionB.IsChecked == true &&
                     question.Options.Count > 1)
            {
                selectedOptionId = question.Options[1].OptionId;
            }
            else if (OptionC.IsChecked == true &&
                     question.Options.Count > 2)
            {
                selectedOptionId = question.Options[2].OptionId;
            }
            else if (OptionD.IsChecked == true &&
                     question.Options.Count > 3)
            {
                selectedOptionId = question.Options[3].OptionId;
            }

            _selectedAnswers[question.QuestionId] =
                selectedOptionId;
        }

        private void NextButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            SaveCurrentAnswer();

            MoveToNextQuestion();
        }

        private async void MoveToNextQuestion()
        {
            _timer.Stop();

            // Save the answer for the current question
            SaveCurrentAnswer();

            if (_currentQuestionIndex < _questions.Count - 1)
            {
                _currentQuestionIndex++;

                ShowQuestion();
                return;
            }

            // Quiz is finished
            int correctAnswers = 0;
            int wrongAnswers = 0;
            int skippedAnswers = 0;

            foreach (Question question in _questions)
            {
                if (!_selectedAnswers.TryGetValue(
                        question.QuestionId,
                        out int? selectedOptionId) ||
                    selectedOptionId == null)
                {
                    skippedAnswers++;
                    continue;
                }

                QuizOption? selectedOption =
                    question.Options.FirstOrDefault(
                        option => option.OptionId == selectedOptionId);

                if (selectedOption != null &&
                    selectedOption.IsCorrect)
                {
                    correctAnswers++;
                }
                else
                {
                    wrongAnswers++;
                }
            }

            int score = correctAnswers;

            try
            {
                await _quizService.SaveQuizAttemptAsync(
                    _loggedInUser.UserId,
                    _categoryId,
                    score,
                    _questions.Count,
                    correctAnswers,
                    wrongAnswers,
                    skippedAnswers,
                    _selectedAnswers);

                ResultWindow resultWindow =
    new ResultWindow(
        _loggedInUser,
        _categoryName,
        score,
        _questions.Count,
        correctAnswers,
        wrongAnswers,
        skippedAnswers);

                resultWindow.Show();

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(),
                    "Quiz Database Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void PreviousButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (_currentQuestionIndex > 0)
            {
                SaveCurrentAnswer();

                _timer.Stop();

                _currentQuestionIndex--;

                ShowQuestion();
            }
        }
    }
}