using System.Collections.Generic;

namespace CybersecurityBot.Bot
{
    public static class TopicMemory
    {
        private static string _currentTopic = "";
        private static readonly Dictionary<string, string> _knownTopics = new Dictionary<string, string>
        {
            { "password", "passwords" },
            { "authentication", "passwords" },
            { "phishing", "phishing" },
            { "scam", "phishing" },
            { "malware", "malware" },
            { "virus", "malware" },
            { "privacy", "privacy" },
            { "safety", "safety" },
            { "vpn", "vpn" },
            { "firewall", "vpn" },
            { "2fa", "authentication" },
            { "mfa", "authentication" }
        };

        public static void SetCurrentTopic(string topic)
        {
            _currentTopic = topic;
        }

        public static string GetCurrentTopic()
        {
            return _currentTopic;
        }

        public static string MatchKnownTopic(string input)
        {
            foreach (var pair in _knownTopics)
            {
                if (input.Contains(pair.Key))
                    return pair.Value;
            }
            return "";
        }
    }
}
