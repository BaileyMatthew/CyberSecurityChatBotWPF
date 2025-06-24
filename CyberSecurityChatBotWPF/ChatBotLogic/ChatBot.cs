using CybersecurityBot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CybersecurityBot.Bot
{
    public class ChatBot
    {
        private readonly ResponseManager responseManager;
        private string currentTopic = null;
        private string lastQuestionAsked = null;
        private bool awaitingYesNo = false;
        private bool awaitingMoodResponse = false;

        // New flag to track yes/no answer specifically for reminders
        private bool awaitingReminderYesNo = false;

        public ChatBot()
        {
            responseManager = new ResponseManager();
        }

        public string GetResponse(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please say something.";

            string input = userInput.ToLower().Trim();

            // ==== HANDLE REMINDER YES/NO FIRST ====
            if (awaitingReminderYesNo)
            {
                if (input.Contains("yes"))
                {
                    awaitingReminderYesNo = false;
                    // You could ask for details here, or user can say "remind me in X days"
                    return "Sure! When would you like me to remind you? Please specify in days (e.g., 'remind me in 3 days').";
                }
                else if (input.Contains("no"))
                {
                    awaitingReminderYesNo = false;
                    return "Perfect, what can I help you with next?";
                }
                else
                {
                    return "Please answer with 'yes' or 'no'. Would you like a reminder?";
                }
            }

            // ==== TASK MANAGEMENT COMMANDS ====

            if (input.StartsWith("add task"))
            {
                string taskText = input.Replace("add task", "").Trim();
                if (string.IsNullOrWhiteSpace(taskText))
                    return "Please provide a title or description for the task.";

                // Add task
                TaskManager.AddTask(taskText, $"Task added: {taskText}");
                // Ask if user wants a reminder
                awaitingReminderYesNo = true;
                return $"Task added with the description \"{taskText}\". Would you like a reminder?";
            }

            if (input.StartsWith("remind me in"))
            {
                if (!TaskManager.ListTasks().Any())
                    return "You need to add a task first.";

                int days = int.TryParse(Regex.Match(input, @"\d+").Value, out int d) ? d : 0;
                if (days <= 0) return "Please enter a valid number of days.";

                // Add reminder to the last task
                var lastTask = typeof(TaskManager).GetField("Tasks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                                                  .GetValue(null) as List<TaskItem>;

                if (lastTask != null && lastTask.Any())
                {
                    lastTask.Last().ReminderDate = DateTime.Now.AddDays(days);
                    return $"Got it! I'll remind you in {days} days.";
                }
                return "Sorry, I couldn't find any tasks to remind you about.";
            }

            if (input == "view tasks")
            {
                var tasks = TaskManager.ListTasks();
                if (!tasks.Any())
                    return "You have no tasks at the moment.";
                return string.Join("\n", tasks);
            }

            if (input.StartsWith("complete task"))
            {
                int taskNum = int.TryParse(Regex.Match(input, @"\d+").Value, out int n) ? n : -1;
                return TaskManager.CompleteTask(taskNum);
            }

            if (input.StartsWith("delete task"))
            {
                int taskNum = int.TryParse(Regex.Match(input, @"\d+").Value, out int n) ? n : -1;
                return TaskManager.DeleteTask(taskNum);
            }

            // ==== EXISTING CHATBOT LOGIC ====

            // Small talk - "how are you"
            if (input.Contains("how are you"))
            {
                awaitingMoodResponse = true;
                return "I'm great, and you?";
            }

            if (awaitingMoodResponse)
            {
                if (input.Contains("good") || input.Contains("fine") || input.Contains("okay"))
                {
                    awaitingMoodResponse = false;
                    return "I'm glad to hear that!";
                }
                else if (input.Contains("bad") || input.Contains("not good") || input.Contains("sad"))
                {
                    awaitingMoodResponse = false;
                    return "Sorry to hear that. Want to talk about it?";
                }
            }

            // Check for yes/no while waiting
            if (awaitingYesNo)
            {
                if (input.Contains("yes"))
                {
                    awaitingYesNo = false;
                    return responseManager.GetFollowUp(currentTopic, true);
                }
                else if (input.Contains("no"))
                {
                    awaitingYesNo = false;
                    return responseManager.GetFollowUp(currentTopic, false);
                }
            }

            // Detect topic + emotion
            string topic = responseManager.DetectTopic(input);
            string emotion = responseManager.DetectEmotion(input);

            if (!string.IsNullOrEmpty(emotion))
            {
                return responseManager.GetEmotionResponse(emotion);
            }

            if (!string.IsNullOrEmpty(topic))
            {
                currentTopic = topic;
                var response = responseManager.GetTopicResponse(topic, input);
                if (responseManager.IsYesNoExpected(response))
                {
                    awaitingYesNo = true;
                    lastQuestionAsked = response;
                }
                return response;
            }

            // Fallback
            return responseManager.GetFallbackResponse();
        }
    }
}
