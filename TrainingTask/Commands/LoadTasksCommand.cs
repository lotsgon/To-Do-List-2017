using ClassLibrary;

namespace TrainingTask
{
    public class LoadTasksCommand : Command
    {
        public LoadTasksCommand()
            : base(title: "Load Tasks") { }

        public override void Execute(TaskCollection tasks)
        {
            if (tasks.LoadFromFile(ConsoleUtilities.ReadString("Enter the name of the file you want to load: ")))
            {
                Program.Status = "ToDoList.xml loaded successfully!";
                return;
            }
            Program.Status = "ToDoList.xml failed to load!";
        }
    }
}