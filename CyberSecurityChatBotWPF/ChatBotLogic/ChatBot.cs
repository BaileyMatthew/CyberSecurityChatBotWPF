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
        private bool awaitingReminderYesNo = false;

        private TaskItem lastTaskAdded = null;

        public ChatBot()
        {
            responseManager = new ResponseManager();
        }

        public string GetResponse(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please say something.";

            string input = userInput.ToLower().Trim();

            // === Show activity log command
            if (Regex.IsMatch(input, @"(show|display|what\s+have\s+you\s+done|activity\s+log|recent\s+actions)", RegexOptions.IgnoreCase))
            {
                return ActivityLogger.GetRecentLog();
            }

            // === Handle "remind me to ___ tomorrow"
            if (input.StartsWith("remind me to") && input.Contains("tomorrow"))
            {
                string taskText = input.Replace("remind me to", "")
                                       .Replace("tomorrow", "")
                                       .Trim();

                if (string.IsNullOrWhiteSpace(taskText))
                    return "What should I remind you to do?";

                DateTime reminderDate = DateTime.Now.AddDays(1);
                TaskManager.AddTask(taskText, taskText, reminderDate);
                string confirmation = $"Reminder set for '{taskText}' on tomorrow's date.";
                ActivityLogger.Log(confirmation);
                return confirmation;
            }

            // === Awaiting yes/no for reminder
            if (awaitingReminderYesNo)
            {
                if (input.Contains("yes"))
                {
                    awaitingReminderYesNo = false;
                    return "Great! When should I remind you? (e.g., say 'remind me in 3 days')";
                }
                else if (input.Contains("no"))
                {
                    awaitingReminderYesNo = false;

                    if (lastTaskAdded != null)
                        ActivityLogger.Log($"Task added: '{lastTaskAdded.Title}' (no reminder set).");

                    lastTaskAdded = null;
                    return "Okay, no reminder set. What next?";
                }
                else
                {
                    return "Please answer with 'yes' or 'no'. Would you like a reminder for this task?";
                }
            }

            // === Set reminder like "remind me in 3 days"
            if (input.StartsWith("remind me in"))
            {
                int days = int.TryParse(Regex.Match(input, @"\d+").Value, out int d) ? d : 0;
                if (days <= 0) return "Please say how many days, like 'remind me in 3 days'.";

                if (lastTaskAdded != null)
                {
                    lastTaskAdded.ReminderDate = DateTime.Now.AddDays(days);
                    string response = $"Reminder set for '{lastTaskAdded.Title}' in {days} day(s).";
                    ActivityLogger.Log(response);
                    lastTaskAdded = null;
                    return response;
                }

                var latest = TaskManager.GetLastTask();
                if (latest != null)
                {
                    latest.ReminderDate = DateTime.Now.AddDays(days);
                    string response = $"Reminder set for '{latest.Title}' in {days} day(s).";
                    ActivityLogger.Log(response);
                    return response;
                }

                return "I couldn't find a task to remind you about.";
            }

            // === Add Task Logic
            if (input.StartsWith("add a task") || input.StartsWith("add task"))
            {
                string taskText = input.Replace("add a task", "")
                                       .Replace("add task", "").Trim();

                if (string.IsNullOrWhiteSpace(taskText))
                    return "Please provide a title for the task.";

                TaskManager.AddTask(taskText, taskText);
                lastTaskAdded = TaskManager.GetLastTask();
                awaitingReminderYesNo = true;
                return $"Task added: '{taskText}'. Would you like to set a reminder for this task?";
            }

            // === View task list
            if (input == "view tasks")
            {
                string result = TaskManager.ListTasks();
                if (!string.IsNullOrWhiteSpace(result))
                    ActivityLogger.Log("Viewed task list.");
                return result;
            }

            // === Complete task
            if (input.StartsWith("complete task"))
            {
                int taskNum = int.TryParse(Regex.Match(input, @"\d+").Value, out int n) ? n : -1;
                string result = TaskManager.CompleteTask(taskNum);
                if (!result.StartsWith("Invalid"))
                    ActivityLogger.Log($"Marked task {taskNum} as complete.");
                return result;
            }

            // === Delete task
            if (input.StartsWith("delete task"))
            {
                int taskNum = int.TryParse(Regex.Match(input, @"\d+").Value, out int n) ? n : -1;
                string result = TaskManager.DeleteTask(taskNum);
                if (!result.StartsWith("Invalid"))
                    ActivityLogger.Log($"Deleted task {taskNum}.");
                return result;
            }

            // === Small talk
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

            // === Follow-up yes/no to a topic
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

            // === NLP-style detection
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

            // === Fallback
            return responseManager.GetFallbackResponse();
        }
    }
}
