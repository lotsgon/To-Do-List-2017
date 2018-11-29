using System;
using System.Linq;
using ClassLibrary;

namespace TrainingTask
{
    public class CreateTaskCommand : Command
    {
        public CreateTaskCommand()
            : base(title: "Create Task") { }

        public override void Execute(TaskCollection tasks)
        {
            var title = ConsoleUtilities.ReadString("Name your task: ");
            var description = ConsoleUtilities.ReadString("Describe your task: ");

            Console.Write("Specify tags (separated by comma): ");

            var tags = Console.ReadLine()
                   .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(tag => tag.Trim());

            if (!tags.Any())
            {
                Console.WriteLine($"{Environment.NewLine}You need to speficy a tag. TRY AGAIN FOOL");

                this.Execute(tasks);

                return;
            }

            tasks.AddTask(title, description, tags.ToList());

            tasks.ActiveTask = tasks.Last();

            Program.Status = $"Task Added! Active task is now {tasks.ActiveTask.Title}";
        }
    }
}
