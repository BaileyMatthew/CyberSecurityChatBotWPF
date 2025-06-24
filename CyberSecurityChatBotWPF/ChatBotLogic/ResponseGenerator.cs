namespace CybersecurityBot.Bot
{
    public static class ResponseGenerator
    {
        public static string GenerateResponse(string topic)
        {
            switch (topic)
            {
                case "passwords":
                    return "A strong password should be at least 12 characters and include a mix of letters, numbers, and symbols. You can also use a password manager.";
                case "phishing":
                    return "Phishing emails often look official. Check the sender address, don't click suspicious links, and never give out personal info.";
                case "malware":
                    return "Malware can harm your system or steal data. Use antivirus software and don't download unknown files.";
                case "privacy":
                    return "Privacy online means protecting your personal information. Use encrypted messaging, VPNs, and review app permissions.";
                case "safety":
                    return "Online safety includes being cautious of what you share and whom you interact with. Keep software updated and avoid unknown links.";
                case "vpn":
                    return "A VPN encrypts your internet traffic and hides your location. It’s useful for privacy and protection on public Wi-Fi.";
                default:
                    return "That's an important topic! Can you tell me a bit more so I can help better?";
            }
        }

        public static string GenerateFollowUp(string topic)
        {
            switch (topic)
            {
                case "passwords":
                    return "Consider using two-factor authentication for extra security. Want tips on managing passwords safely?";
                case "phishing":
                    return "There are spear-phishing attacks too, which are more targeted. Would you like to learn how to report phishing?";
                case "malware":
                    return "Some malware types include ransomware and spyware. Need help identifying malware on your device?";
                case "privacy":
                    return "You can enhance privacy with secure browsers and tracking blockers. Want suggestions on tools?";
                case "safety":
                    return "Using parental controls and safe search features helps kids stay safe online. Want to know more?";
                case "vpn":
                    return "Free VPNs may log your data. It's safer to choose a reputable paid VPN. Need help picking one?";
                default:
                    return "Would you like to go deeper into this topic or switch to something else?";
            }
        }
    }
}
