using ClassLibrary;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace TrainingTask
{
    public class Program
    {
        static void Main(string[] args)
        {
            Menu menu = SetupMenu();

            var tasks = new TaskCollection();

            Console.Title = $"Tom's Task Manager ({tasks.Count} tasks in active list)";

            while (true)
            {
                System.Console.WriteLine(Program.Status);
                menu = menu.DisplayMenu(tasks);
                System.Console.Clear();
                Console.Title = $"Tom's Task Manager ({tasks.Count} tasks in active list)";
            }
        }
        
        public static string Status { get; set; } = "Helpful messages will appear here!";

        private static Menu SetupMenu()
        {
            var main = new Menu("Main Menu", null);
            var editTaskMenu = new Menu("Edit Task", null) { Parent = main, RequiresTasks = true };

            var editTaskTagsMenu =
                    new Menu("Edit Tags", CreateOptions(new IMenuOption[]  {
                                    new AddTaskTagsCommand(),
                                    new DeleteTaskTagsCommand() }))
                    {
                        Parent = editTaskMenu
                    };

            var showTasksMenu =
                    new Menu("Show Tasks", CreateOptions(new IMenuOption[] {
                                    new ShowAllTasksCommand(),
                                    new ShowCompletedTasksCommand(),
                                    new ShowIncompleteTasksCommand(),
                                    new ShowTaggedTasksCommand() }))
                    {
                        Parent = main,
                        RequiresTasks = true
                    };

            editTaskMenu.InitializeOptions(CreateOptions(new IMenuOption[] {
                                    new SetActiveTaskCommand(),
                                    new EditTaskTitleCommand(),
                                    new EditTaskDescriptionCommand(),
                                    editTaskTagsMenu
                                    }));
            main.InitializeOptions(CreateOptions(new IMenuOption[] {
                                    new CreateTaskCommand(),
                                    editTaskMenu, 
                                    new DeleteTaskCommand(),
                                    showTasksMenu,
                                    new MarkTaskCommand(),
                                    new SaveTasksCommand(),
                                    new LoadTasksCommand(),
                                    new OrderTasksCommand(),
                                    new ClearTaskListCommand(),
                                    new ExitCommand()
                    }));

            return main;
        }

        private static IReadOnlyDictionary<int, IMenuOption> CreateOptions(IEnumerable<IMenuOption> options)
        {
            var result = new Dictionary<int, IMenuOption>();

            int counter = 1;
            foreach (var option in options)
            {
                result.Add(counter, option);

                counter++;
            }

            return new ReadOnlyDictionary<int, IMenuOption>(result);
        }
    }
}
