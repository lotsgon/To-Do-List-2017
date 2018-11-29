using ClassLibrary;

namespace TrainingTask
{
    class SetActiveTaskCommand : Command
    {
        public SetActiveTaskCommand() 
            : base("Set Active Task") { }

        public override void Execute(TaskCollection tasks)
        {
            if (tasks.Count == 0)
            {
                Program.Status = "No tasks found in list";
                return; 
            }

            tasks.ActiveTask = ConsoleUtilities.PickTask(tasks);
            if (tasks.ActiveTask != null)
            {
                Program.Status = $"Active task set to '{tasks.ActiveTask.Title}'";
            }
            else
            {
                Program.Status = "Selected task not found in list!";
            }
        }
    }
}
