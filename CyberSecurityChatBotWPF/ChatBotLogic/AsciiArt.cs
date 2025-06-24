using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CybersecurityBot
{
    public static class AsciiArt
    {
        public static void DisplayToChatDisplay(TextBlock chatDisplay)
        {
            string ascii = @"
╔══════════════════════════════╗
║   [==== CYBER ====]         ║
║     _________               ║
║    |  O   O  |              ║
║    |    >    |              ║
║    |  \___/  |              ║
║    |_________|              ║
╚══════════════════════════════╝

Cybersecurity Awareness Bot
";

            

       
            Run asciiRun = new Run(ascii)
            {
                Foreground = Brushes.Green,
                FontFamily = new FontFamily("Consolas"),
                FontSize = 14
            };
            chatDisplay.Inlines.Add(asciiRun);
            
        }
    }
}
