using System;
using ClassLibrary;

namespace TrainingTask
{
    public class OrderTasksCommand : Command
    {
        public OrderTasksCommand()
             : base(title: "Order Task List") { RequiresTasks = true; }

        public override void Execute(TaskCollection tasks)
        {
            if (tasks.Count != 0)
            {
                var taskItem = ConsoleUtilities.PickTask(tasks);

                int oldIndex = tasks.IndexOf(taskItem);
                int newIndex = ReadPositionToMove(tasks);

                tasks.Move(oldIndex, newIndex);

                tasks.ForEach(t => t.ShowShort());
            }
        }

        private int ReadPositionToMove(TaskCollection tasks)
        {
            int newIndex;
            while (ConsoleUtilities.TryReadInt("What position do you want to move this task to?", out newIndex))
            {
                if (newIndex <= tasks.Count && newIndex > 0)
                {
                    break;
                }
                Console.WriteLine("Not a valid position!");
            }
            return newIndex;
        }
    }
}