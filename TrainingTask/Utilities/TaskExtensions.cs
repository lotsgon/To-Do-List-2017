using ClassLibrary;
using System;
using System.Text;

namespace TrainingTask
{
    public static class TaskExtensions
    {
        public static void Show(this Task task)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Task Number: {task.Index+1}");
            sb.AppendLine($"Title: {task.Title}");
            sb.AppendLine($"Description: {task.Description}");
            sb.AppendLine($"Tags: {String.Join(", ", task.Tags)}");
            sb.AppendLine($"Completed: {task.Completed}");

            Console.WriteLine(sb.ToString());
        }

        public static void ShowShort(this Task task)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Task Number: {task.Index + 1}");
            sb.AppendLine($"Title: {task.Title}");
            sb.AppendLine($"Completed: {task.Completed}");

            Console.WriteLine(sb.ToString());
        }
    }
}
