using ClassLibrary;

namespace TrainingTask
{
    class EditTaskTitleCommand : Command
    {
        public EditTaskTitleCommand()
            : base(title: "Edit Title") { }

        public override void Execute(TaskCollection tasks)
        {
            if (tasks.ActiveTask != null)
            {
                tasks.ActiveTask.Title = ConsoleUtilities.ReadString("Name your task: ");
            }
            else
            {
                Program.Status = "There is no active task set, please set one!";
            }
        }
    }
}
