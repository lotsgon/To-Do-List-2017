using System;
using System.Linq;
using ClassLibrary;

namespace TrainingTask
{
    class ShowCompletedTasksCommand : Command
    {

        public ShowCompletedTasksCommand()
            : base(title: "Show Completed Tasks") { }

        public override void Execute(TaskCollection tasks)
        {
            if (tasks.Count == 0)
            {
                Program.Status = "The task list is empty. Please add some tasks!";

                return;
            }

            if(!tasks.Any(t => t.Completed))
            {
                Console.WriteLine("Nothing's complete!");
                return;
            }

            tasks.ForEach(task =>
            {
                if (task.Completed)
                {
                    task.Show();
                }
                //Console.WriteLine("No task found with that tag!");
            });

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
    }
}
