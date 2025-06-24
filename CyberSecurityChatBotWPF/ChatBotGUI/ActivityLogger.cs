using System;
using System.Collections.Generic;
using System.Text;

public static class ActivityLogger
{
    private static readonly Queue<string> _logEntries = new Queue<string>();
    private static readonly object _lock = new object();
    private const int MaxEntries = 10;

    public static void Log(string message)
    {
        lock (_lock)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string entry = $"{timestamp} - {message}";
            if (_logEntries.Count >= MaxEntries)
                _logEntries.Dequeue();
            _logEntries.Enqueue(entry);
        }
    }

    public static string GetRecentLog(int maxEntries = MaxEntries)
    {
        lock (_lock)
        {
            var entries = _logEntries.ToArray();
            int start = entries.Length > maxEntries ? entries.Length - maxEntries : 0;
            var sb = new StringBuilder("Here's a summary of recent actions:\n");
            for (int i = start; i < entries.Length; i++)
            {
                sb.AppendLine($"{i - start + 1}. {entries[i]}");
            }
            if (entries.Length == 0)
                return "No recent activity to show.";
            return sb.ToString().TrimEnd();
        }
    }
}
