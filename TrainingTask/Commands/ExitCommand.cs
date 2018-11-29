using System;
using ClassLibrary;

namespace TrainingTask
{
    class ExitCommand : Command
    {
        public ExitCommand() : base("Exit Program") { }

        public override void Execute(TaskCollection tasks)
        {
            Environment.Exit(0);
        }
    }
}
