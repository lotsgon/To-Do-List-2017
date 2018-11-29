using ClassLibrary;

namespace TrainingTask
{
    public class MarkTaskCommand : Command
    {
        public MarkTaskCommand()
            : base(title: "Mark/Unmark Task") { RequiresTasks = true; }
        
        public override void Execute(TaskCollection tasks)
        {
            if (tasks.Count != 0)
            {
                Task task = ConsoleUtilities.PickTask(tasks);
                task.Completed = !task.Completed;
                if (task.Completed)
                {
                    Program.Status = "Task marked!";
                }
                Program.Status = "Task unmarked!";
            }
        }
    }
}