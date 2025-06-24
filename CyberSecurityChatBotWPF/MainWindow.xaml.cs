using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CybersecurityBot.Bot;
using CyberSecurityChatBotWPF.Views;

namespace CyberSecurityChatBotWPF
{
    public partial class MainWindow : Window
    {
        private ChatBot bot;

        public MainWindow()
        {
            InitializeComponent();
            bot = new ChatBot();
            AppendBotMessage("Hello! I'm your Cybersecurity Assistant. 💻\nAsk me anything about phishing, malware, VPNs, and more.");
            Loaded += (s, e) => UserInput.Focus();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string input = UserInput.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            AppendUserMessage(input);
            string response = bot.GetResponse(input);
            await TypeBotMessageAsync(response);

            UserInput.Clear();
            UserInput.Focus();
        }

        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            var quizWindow = new QuizWindow();
            quizWindow.ShowDialog();
            AppendBotMessage("Hope you enjoyed the quiz! Ask me anything else you'd like to know.");
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendButton_Click(sender, e);
                e.Handled = true;
            }
        }

        private void AppendUserMessage(string message)
        {
            ChatDisplay.Text += $"\n👤 You: {message}\n";
            ScrollToEnd();
        }

        private void AppendBotMessage(string message)
        {
            ChatDisplay.Text += $"🤖 Bot: {message}\n";
            ScrollToEnd();
        }

        private async Task TypeBotMessageAsync(string message)
        {
            ChatDisplay.Text += "🤖 Bot: ";
            foreach (char c in message)
            {
                ChatDisplay.Text += c;
                ScrollToEnd();
                await Task.Delay(20);
            }
            ChatDisplay.Text += "\n";
        }

        private void ScrollToEnd()
        {
            ScrollArea.ScrollToEnd();
        }
    }
}
