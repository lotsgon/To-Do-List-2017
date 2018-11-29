using ClassLibrary;

namespace TrainingTask
{
    public abstract class Command : IMenuOption
    {
        private readonly string mOriginalTitle;

        public Command(string title)
        {
            mOriginalTitle = title;
            Title = title;
        }

        public string Title { get; private set; }

        public bool RequiresTasks { get; set; }

        public bool CanSelect { get; set; } = true;

        public abstract void Execute(TaskCollection tasks);

        public virtual void CheckPrerequisites(TaskCollection tasks)
        {
            if (this.RequiresTasks)
            {
                if (tasks.Count == 0)
                {
                    this.Title = $"{mOriginalTitle} - You have no tasks";
                    this.CanSelect = false;
                }
                else
                {
                    this.Title = mOriginalTitle;
                    this.CanSelect = true;
                }
            }
            else
            {
                this.CanSelect = true;
            }
        }
    }
}
