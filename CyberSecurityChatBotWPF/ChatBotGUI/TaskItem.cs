using System;

public class TaskItem
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime? ReminderDate { get; set; }
    public bool IsCompleted { get; set; }

    public override string ToString()
    {
        string status = IsCompleted ? "✅ Done" : "❌ Pending";
        string reminder = ReminderDate.HasValue ? $" (Reminder: {ReminderDate.Value:yyyy-MM-dd})" : "";
        return $"{Title} - {Description} [{status}]{reminder}";
    }
}
