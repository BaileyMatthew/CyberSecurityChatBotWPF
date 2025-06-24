using System.Linq;
using System.Windows;
using CybersecurityBot.Helpers;

namespace CyberSecurityChatBotWPF.Views
{
    public partial class QuizWindow : Window
    {
        private readonly CyberQuizManager quizManager;

        public QuizWindow()
        {
            InitializeComponent();
            quizManager = new CyberQuizManager();
            quizManager.StartQuiz();
            LoadQuestion();
        }

        private void LoadQuestion()
        {
            string fullQuestion = quizManager.GetNextQuestion();

            if (!quizManager.IsQuizActive)
            {
                QuestionText.Text = fullQuestion;
                AnswerList.ItemsSource = null;
                return;
            }

            string[] lines = fullQuestion.Split('\n');
            QuestionText.Text = lines[0];
            AnswerList.ItemsSource = lines.Skip(1).ToArray(); // Compatible with C# 7.3

        }

        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (AnswerList.SelectedItem == null)
            {
                FeedbackText.Text = "Please select an answer.";
                return;
            }

            string selected = AnswerList.SelectedItem.ToString();
            string selectedLetter = selected.Substring(0, 1).ToUpper(); // e.g., 'A'
            string feedback = quizManager.SubmitAnswer(selectedLetter);

            FeedbackText.Text = feedback;
            LoadQuestion();
        }
    }
}
