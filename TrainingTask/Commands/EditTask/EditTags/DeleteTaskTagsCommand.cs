using System;
using ClassLibrary;

namespace TrainingTask
{
    class DeleteTaskTagsCommand : Command
    {
        public DeleteTaskTagsCommand()
            : base(title: "Delete Tags") { }

        public override void Execute(TaskCollection tasks)
        {
            if (tasks.ActiveTask == null)
            {
                Program.Status = "There is no active task set, please set one!";
            }

            Console.WriteLine($"Task number: {tasks.IndexOf(tasks.ActiveTask) + 1}.");
            tasks.ActiveTask.Show();

            var tag = ConsoleUtilities.ReadString("Remove a tag from your task:");

            if (tasks.ActiveTask.Tags.Count < 1)
            {
                Program.Status = "Tag list must have one tag!";
                return;
            }

            tasks.ActiveTask.Tags.Remove(tag);
        }
    }
}
