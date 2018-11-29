using ClassLibrary;

namespace TrainingTask
{
    public class SaveTasksCommand : Command
    {
        public SaveTasksCommand()
             : base(title: "Save Task List") { RequiresTasks = true; }
        
        public override void Execute(TaskCollection tasks)
        {
            if (tasks.SaveToFile(ConsoleUtilities.ReadString("Enter a name for your file: ")))
            {
                Program.Status = "Task list saved successfully!";
                return;
            }
            Program.Status = "There is already a file with that name!";
        }
    }
}