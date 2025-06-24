using System.Collections.Generic;

namespace CybersecurityBot.Bot
{
    public enum Emotion { Neutral, Angry, Worried, Curious, Happy }

    public static class EmotionAnalyzer
    {
        private static readonly Dictionary<string, Emotion> _emotionKeywords = new Dictionary<string, Emotion>
        {
            { "angry", Emotion.Angry },
            { "mad", Emotion.Angry },
            { "frustrated", Emotion.Angry },
            { "worried", Emotion.Worried },
            { "scared", Emotion.Worried },
            { "concerned", Emotion.Worried },
            { "curious", Emotion.Curious },
            { "interested", Emotion.Curious },
            { "happy", Emotion.Happy },
            { "glad", Emotion.Happy }
        };

        public static Emotion DetectEmotion(string input)
        {
            foreach (var pair in _emotionKeywords)
            {
                if (input.Contains(pair.Key))
                    return pair.Value;
            }
            return Emotion.Neutral;
        }

        public static string RespondToEmotion(Emotion emotion)
        {
            switch (emotion)
            {
                case Emotion.Angry:
                    return "I understand you're upset. Cybersecurity issues can be frustrating—I'm here to help.";
                case Emotion.Worried:
                    return "Don't worry, you're not alone. Let's work through your concern together.";
                case Emotion.Curious:
                    return "I'm glad you're interested! Ask away—learning about cybersecurity is powerful.";
                case Emotion.Happy:
                    return "That's great to hear! Let's keep your online world safe and secure.";
                default:
                    return "";
            }
        }
    }
}
