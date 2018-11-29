using System;
using ClassLibrary;

namespace TrainingTask
{
    class ShowIncompleteTasksCommand : Command
    {
        public ShowIncompleteTasksCommand()
            : base(title: "Show Incompleted Tasks") { }

        public override void Execute(TaskCollection tasks)
        {
            if (tasks.Count == 0)
            {
                Program.Status = "The task list is empty. Please add some tasks!";

                return;
            }

            tasks.ForEach(task =>
            {
                if (!task.Completed)
                {
                    task.Show();
                }
                Console.WriteLine("No task found with that tag!");
            });

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
    }
}
