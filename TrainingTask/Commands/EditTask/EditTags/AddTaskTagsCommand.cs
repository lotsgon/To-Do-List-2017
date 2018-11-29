using System;
using System.Linq;
using ClassLibrary;

namespace TrainingTask
{
    class AddTaskTagsCommand : Command
    {
        public AddTaskTagsCommand()
             : base(title: "Add Tags") { }

        public override void Execute(TaskCollection tasks)
        {
            if (tasks.ActiveTask == null)
            {
                Program.Status = "There is no active task set, please set one!";
                return;
            }

            Console.WriteLine($"Task number: {tasks.IndexOf(tasks.ActiveTask)}.");
            tasks.ActiveTask.Show();

            Console.Write("Specify tags (separated by comma): ");

            var tags
                = Console.ReadLine()
                               .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Take(5)
                                .Select(tag => tag.Trim());

            if (!tags.Any())
            {
                return;
            }

            tasks.ActiveTask.Tags.AddRange(tags);
        }
    }
}
