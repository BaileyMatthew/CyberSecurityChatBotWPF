using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace CybersecurityBot.Helpers
{
    public static class TextEffects
    {
        // Typing animation into a TextBox (multiline)
        public static async Task TypeLineAsync(TextBox outputBox, string text, int delay = 20, Brush color = null)
        {
            foreach (char c in text)
            {
                outputBox.AppendText(c.ToString());
                outputBox.ScrollToEnd();
                await Task.Delay(delay);
            }
            outputBox.AppendText("\n");
        }

        public static void SectionHeader(TextBox outputBox, string title)
        {
            string line = new string('=', 60);
            outputBox.AppendText($"\n{line}\n{title.ToUpper()}\n{line}\n");
            outputBox.ScrollToEnd();
        }

        public static void Divider(TextBox outputBox)
        {
            outputBox.AppendText(new string('-', 60) + "\n");
            outputBox.ScrollToEnd();
        }
    }
}
