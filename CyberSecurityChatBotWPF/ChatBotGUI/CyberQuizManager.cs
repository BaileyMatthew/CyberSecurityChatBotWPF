// CyberQuizManager.cs
using System;
using System.Collections.Generic;

namespace CybersecurityBot.Helpers
{
    public class CyberQuizManager
    {
        private readonly List<QuizQuestion> questions;
        private int currentQuestionIndex;
        private int score;
        public bool IsQuizActive { get; private set; }

        public CyberQuizManager()
        {
            questions = GetQuizQuestions();
            currentQuestionIndex = 0;
            score = 0;
            IsQuizActive = false;
        }

        public void StartQuiz()
        {
            IsQuizActive = true;
            currentQuestionIndex = 0;
            score = 0;
        }

        public string GetNextQuestion()
        {
            if (currentQuestionIndex >= questions.Count)
                return EndQuiz();

            return questions[currentQuestionIndex].FormattedQuestion();
        }

        public string SubmitAnswer(string userAnswer)
        {
            var question = questions[currentQuestionIndex];
            string feedback;

            if (question.IsCorrect(userAnswer))
            {
                score++;
                feedback = $"Correct! {question.Explanation}";
            }
            else
            {
                feedback = $"Incorrect. {question.Explanation}";
            }

            currentQuestionIndex++;

            if (currentQuestionIndex < questions.Count)
                return feedback + "\n\nNext Question:\n" + GetNextQuestion();
            else
                return feedback + "\n\n" + EndQuiz();
        }

        private string EndQuiz()
        {
            IsQuizActive = false;
            string result = $"Quiz complete! Your score: {score}/{questions.Count}\n";

            if (score >= 8)
                result += "Great job! You're a cybersecurity pro!";
            else if (score >= 5)
                result += "Not bad! Keep learning to stay safe online.";
            else
                result += "You can do better! Study up to protect yourself online.";

            return result;
        }

        private List<QuizQuestion> GetQuizQuestions()
        {
            return new List<QuizQuestion>
            {
                new QuizQuestion("What should you do if you receive an email asking for your password?", new[] {"A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it"}, "C", "Reporting phishing emails helps prevent scams."),
                new QuizQuestion("True or False: It's safe to use the same password for all accounts.", new[] {"A) True", "B) False"}, "B", "Using different passwords prevents one breach from affecting all your accounts."),
                new QuizQuestion("What is phishing?", new[] {"A) A hacking tool", "B) An antivirus", "C) A fake attempt to steal info", "D) A safe app"}, "C", "Phishing is a scam to trick you into giving personal info."),
                new QuizQuestion("Which password is strongest?", new[] {"A) Password123", "B) qwerty", "C) LetMeIn", "D) 8u@T!9z#Xq"}, "D", "Strong passwords use a mix of letters, numbers, and symbols."),
                new QuizQuestion("True or False: Public Wi-Fi is always secure.", new[] {"A) True", "B) False"}, "B", "Public Wi-Fi is often unsecured and risky to use without a VPN."),
                new QuizQuestion("Which of these is a sign of a phishing website?", new[] {"A) Padlock icon in address bar", "B) Strange URL or spelling errors", "C) HTTPS", "D) Fast loading"}, "B", "Phishing sites often have weird URLs or bad grammar."),
                new QuizQuestion("What should you do if your device is infected with malware?", new[] {"A) Ignore it", "B) Use antivirus software", "C) Share the file", "D) Restart only"}, "B", "Antivirus software can detect and remove malware."),
                new QuizQuestion("True or False: You should click on pop-ups offering free prizes.", new[] {"A) True", "B) False"}, "B", "Such pop-ups are often scams or malware traps."),
                new QuizQuestion("What is two-factor authentication?", new[] {"A) Using password twice", "B) Logging in on 2 devices", "C) Extra verification step", "D) Ignoring verification"}, "C", "It adds an extra step to protect your account."),
                new QuizQuestion("Why is software updating important?", new[] {"A) To change the theme", "B) To get new emojis", "C) To fix bugs and patch security", "D) It’s not important"}, "C", "Updates patch vulnerabilities that hackers might exploit.")
            };
        }
    }

    public class QuizQuestion
    {
        public string Question { get; }
        public string[] Options { get; }
        public string CorrectOption { get; }
        public string Explanation { get; }

        public QuizQuestion(string question, string[] options, string correctOption, string explanation)
        {
            Question = question;
            Options = options;
            CorrectOption = correctOption.ToUpper();
            Explanation = explanation;
        }

        public string FormattedQuestion()
        {
            return $"{Question}\n" + string.Join("\n", Options);
        }

        public bool IsCorrect(string userAnswer)
        {
            return userAnswer.Trim().ToUpper() == CorrectOption;
        }
    }
}
