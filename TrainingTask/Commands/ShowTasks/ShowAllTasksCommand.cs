using System;
using ClassLibrary;

namespace TrainingTask
{
    class ShowAllTasksCommand : Command
    {
        public ShowAllTasksCommand()
            : base(title: "Show All Tasks") { }

        public override void Execute(TaskCollection tasks)
        {
            if (tasks.Count == 0)
            {
                Program.Status = "The task list is empty. Please add some tasks!";

                return;
            }

            tasks.ForEach(task => task.Show());

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
    }
}
