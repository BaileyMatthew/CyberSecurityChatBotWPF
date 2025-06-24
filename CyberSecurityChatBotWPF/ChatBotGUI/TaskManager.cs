using System;
using System.Collections.Generic;
using System.Linq;

public static class TaskManager
{
    private static readonly List<TaskItem> Tasks = new List<TaskItem>();

    public static void AddTask(string title, string description, DateTime? reminder = null)
    {
        Tasks.Add(new TaskItem
        {
            Title = title,
            Description = description,
            ReminderDate = reminder,
            IsCompleted = false
        });

        if (reminder.HasValue)
            ActivityLogger.Log($"Task added: '{title}' (Reminder set for {reminder.Value:yyyy-MM-dd}).");
        else
            ActivityLogger.Log($"Task added: '{title}'.");
    }

    public static string ListTasks()
    {
        if (!Tasks.Any()) return "You have no tasks yet.";

        string result = "";
        int i = 1;
        foreach (var task in Tasks)
        {
            string status = task.IsCompleted ? "✅" : "❌";
            string reminder = task.ReminderDate.HasValue
                ? $" | Reminder: {task.ReminderDate.Value:yyyy-MM-dd}"
                : "";

            result += $"{i}. {task.Title} - {task.Description}{reminder} [{status}]\n";
            i++;
        }

        return result;
    }

    public static string CompleteTask(int index)
    {
        if (index < 1 || index > Tasks.Count)
            return "Invalid task number.";

        Tasks[index - 1].IsCompleted = true;
        ActivityLogger.Log($"Task {index} marked as completed.");
        return $"Task {index} marked as completed.";
    }

    public static string DeleteTask(int index)
    {
        if (index < 1 || index > Tasks.Count)
            return "Invalid task number.";

        var title = Tasks[index - 1].Title;
        Tasks.RemoveAt(index - 1);
        ActivityLogger.Log($"Task {index} ('{title}') deleted.");
        return $"Task {index} deleted.";
    }

    public static TaskItem GetLastTask()
    {
        return Tasks.LastOrDefault();
    }

    public static bool HasTasks()
    {
        return Tasks.Any();
    }
}
