using ClassLibrary;

namespace TrainingTask
{
    class EditTaskDescriptionCommand : Command
    {
        public EditTaskDescriptionCommand()
            : base(title: "Edit Description") { }

        public override void Execute(TaskCollection tasks)
        {
            if (tasks.ActiveTask != null)
            {
                tasks.ActiveTask.Description = ConsoleUtilities.ReadString("Describe your task: ");
            }
            else
            {
                Program.Status = "There is no active task set, please set one!";
            }
        }
    }
}
