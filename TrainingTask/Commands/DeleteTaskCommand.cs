using ClassLibrary;

namespace TrainingTask
{
    public class DeleteTaskCommand : Command
    {
        public DeleteTaskCommand()
            : base(title: "Delete Task") { RequiresTasks = true; }
        
        public override void Execute(TaskCollection tasks)
        {
            if (tasks.Count == 0)
            {
                Program.Status = "You don't have any tasks!";
                return;
            }

            tasks.RemoveAt(tasks.IndexOf(ConsoleUtilities.PickTask(tasks)));
            Program.Status = "Task Deleted!";
        }
    }
}