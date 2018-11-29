using ClassLibrary;

namespace TrainingTask
{
    public interface IMenuOption
    {
        string Title { get; }

        bool CanSelect { get; set; }

        void CheckPrerequisites(TaskCollection tasks);
    }
}
