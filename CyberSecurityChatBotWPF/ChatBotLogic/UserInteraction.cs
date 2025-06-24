namespace CybersecurityBot.Bot
{
    public static class UserInteraction
    {
        public static string UserName { get; private set; } = "User";

        public static string Greet(string name)
        {
            UserName = string.IsNullOrWhiteSpace(name) ? "User" : name;
            return $" Welcome, {UserName}! I'm here to help you stay safe online.";
        }

        public static string Farewell()
        {
            return $" Goodbye, {UserName}! Stay safe out there.";
        }
    }
}
