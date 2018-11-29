using System;
using System.Linq;
using ClassLibrary;

namespace TrainingTask
{
    class ShowTaggedTasksCommand : Command
    {
        public ShowTaggedTasksCommand()
            : base(title: "Search For Tagged Tasks") { }

        public override void Execute(TaskCollection tasks)
        {
            if (tasks.Count == 0)
            {
                Program.Status = "The task list is empty. Please add some tasks!";

                return;
            }

            var tagToFind = ConsoleUtilities.ReadString("Please enter the tag you would like to search for: ");

            var tasksWithTag = tasks.Where(t => t.Tags.Contains(tagToFind));

            if (!tasksWithTag.Any())
            {
                Console.WriteLine("No task found with that tag!");
            }
            else
            {
                tasks.ForEach(task => task.Show());
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
    }
}
