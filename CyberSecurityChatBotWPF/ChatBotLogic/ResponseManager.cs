using System;
using System.Collections.Generic;
using System.Linq;

public class ResponseManager
{
    private readonly Dictionary<string, List<string>> topicKeywords = new Dictionary<string, List<string>>
    {
        { "phishing", new List<string> { "phishing", "phish", "email scam" } },
        { "malware", new List<string> { "malware", "virus", "trojan", "spyware" } },
        { "vpn", new List<string> { "vpn", "virtual private network" } },
        { "passwords", new List<string> { "password", "passcode", "login credentials" } },
        { "privacy", new List<string> { "privacy", "data protection", "tracking", "ads" } },
        { "social engineering", new List<string> { "social engineering", "trick", "manipulate", "impersonate" } }
    };

    private readonly Dictionary<string, string> topicResponses = new Dictionary<string, string>
    {
        { "phishing", "Phishing emails often look official. Check the sender address, don't click suspicious links, and never give out personal info. Would you like to learn how to report phishing?" },
        { "malware", "Malware can harm your system or steal data. Use antivirus software and don't download unknown files. Would you like more tips on malware protection?" },
        { "vpn", "A VPN encrypts your internet traffic and hides your location. It's useful for privacy and protection on public Wi-Fi. Would you like help choosing a VPN?" },
        { "passwords", "Strong passwords are key to online security. Use a mix of letters, numbers, and symbols. Would you like tips on password managers?" },
        { "privacy", "Online privacy matters. Limit app permissions, use privacy settings on social media, and consider using a VPN. Want to hear more?" },
        { "social engineering", "Social engineering tricks people into giving up information. Always verify requests for sensitive data. Want to know common scams?" }
    };

    private readonly Dictionary<string, (string yes, string no)> followUps = new Dictionary<string, (string yes, string no)>
    {
        { "phishing", ("To report phishing, forward the email to report@phishing.gov.", "Alright, stay alert and be cautious online.") },
        { "malware", ("Make sure your antivirus is updated and run a full system scan regularly.", "No problem. Let me know if you have more questions.") },
        { "vpn", ("Look for a paid VPN with a no-logs policy. NordVPN or ExpressVPN are good choices.", "Okay, just avoid free VPNs that may sell your data.") },
        { "passwords", ("Password managers like Bitwarden or 1Password help store complex passwords safely.", "Alright, just make sure your passwords are hard to guess.") },
        { "privacy", ("Review privacy settings on all your accounts and use tracker blockers.", "No worries. Just be cautious with what you share online.") },
        { "social engineering", ("Be cautious of unexpected calls or emails asking for info. Always verify identities.", "Okay, just stay alert and trust your instincts.") }
    };

    private readonly Dictionary<string, string> emotionResponses = new Dictionary<string, string>
    {
        { "angry", "I'm sorry you're feeling that way. Let's look at ways to improve your online security." },
        { "worried", "Don't worry, you're not alone. Let's work through your concern together." },
        { "frustrated", "I understand your frustration. Let's solve this one step at a time." }
    };

    private readonly List<string> fallbackResponses = new List<string>
    {
        "Could you rephrase that? I’ll do my best to help.",
        "I'm not sure I understand. You can ask about phishing, malware, VPNs, and more.",
        "Let me know if you want help with online safety, passwords, or cyber threats."
    };

    public string DetectTopic(string input)
    {
        foreach (var topic in topicKeywords)
        {
            if (topic.Value.Any(keyword => input.ToLower().Contains(keyword)))
            {
                return topic.Key;
            }
        }
        return null;
    }

    public string GetTopicResponse(string topic, string input)
    {
        return topicResponses.ContainsKey(topic)
            ? topicResponses[topic]
            : "That's an important topic. Let’s discuss it!";
    }

    public bool IsYesNoExpected(string response)
    {
        return response.Contains("Would you like") || response.Contains("Want to");
    }

    public string GetFollowUp(string topic, bool userSaidYes)
    {
        if (followUps.ContainsKey(topic))
        {
            return userSaidYes ? followUps[topic].yes : followUps[topic].no;
        }
        return "Thanks for your input.";
    }

    public string DetectEmotion(string input)
    {
        input = input.ToLower();
        if (input.Contains("angry") || input.Contains("mad") || input.Contains("furious")) return "angry";
        if (input.Contains("worried") || input.Contains("anxious") || input.Contains("scared")) return "worried";
        if (input.Contains("frustrated") || input.Contains("upset")) return "frustrated";
        return null;
    }

    public string GetEmotionResponse(string emotion)
    {
        return emotionResponses.ContainsKey(emotion)
            ? emotionResponses[emotion]
            : "I'm here to help however I can.";
    }

    public string GetFallbackResponse()
    {
        var random = new Random();
        return fallbackResponses[random.Next(fallbackResponses.Count)];
    }
}
