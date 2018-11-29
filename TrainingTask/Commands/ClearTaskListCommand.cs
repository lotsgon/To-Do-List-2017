using System;
using ClassLibrary;

namespace TrainingTask
{
    class ClearTaskListCommand : Command
    {
        public ClearTaskListCommand() 
            : base("Clear task list") { RequiresTasks = true; }

        public override void Execute(TaskCollection tasks)
        {
            Console.Write("Are you sure? (y/N): ");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                tasks.Clear();

                Program.Status = "Task list cleared!";
                return;
            }
            Program.Status = "You didn't say yes, the list has not been cleared.";
        }
    }
}
