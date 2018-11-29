using ClassLibrary;
using System;
using System.Collections.Generic;

namespace TrainingTask
{
    public class Menu : IMenuOption
    {
        private readonly string mOriginalTitle;
        private IReadOnlyDictionary<int, IMenuOption> mMenuOptions;

        public Menu(string title)
        {
            mOriginalTitle = title;
            this.Title = mOriginalTitle;
        }

        public Menu(string title, IReadOnlyDictionary<int, IMenuOption> commands)
            : this(title)
        {
            mMenuOptions = commands;
        }

        public string Title { get; private set; }

        public Menu Parent { get; set; }

        public bool RequiresTasks { get; set; }

        public bool CanSelect { get; set; }

        public void InitializeOptions(IReadOnlyDictionary<int, IMenuOption> options)
        {
            mMenuOptions = options;
        }

        public void CheckPrerequisites(TaskCollection tasks)
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

        internal Menu DisplayMenu(TaskCollection tasks)
        {
            Console.WriteLine("==========================");
            Console.WriteLine($"{this.Title}");
            foreach (var menuOption in mMenuOptions)
            {
                menuOption.Value.CheckPrerequisites(tasks);

                if (menuOption.Value.CanSelect)
                {
                    Console.WriteLine($"{menuOption.Key}. { menuOption.Value.Title}");
                }
                else
                {
                    var old = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{menuOption.Key}. { menuOption.Value.Title}");
                    Console.ForegroundColor = old;
                }
            }

            if (this.Parent != null)
            {
                Console.WriteLine($"{mMenuOptions.Count + 1}. Go Back");
            }

            Console.WriteLine("==========================");

            if (!int.TryParse(ConsoleUtilities.ReadString("Please select an option:"), out int i))
            {
                Console.WriteLine("Invalid command, please try again!");
                return this;
            }

            Console.Write("\r\n");

            IMenuOption option = null;
            if (!mMenuOptions.TryGetValue(i, out option))
            {
                if (i == mMenuOptions.Count + 1 && this.Parent != null)
                {
                    return this.Parent;
                }

                Console.WriteLine("That's not an option, pal");
                return this;
            }

            if (!option.CanSelect)
            {
                // This menu option is disabled, pretend nothing happened and go back to the same menu
                return this;
            }

            switch (option)
            {
                case Menu m:
                    return m;
                case Command c:
                    c.Execute(tasks);
                    return this;
                default:
                    System.Diagnostics.Debug.Fail($"Unexpected menu option {option.GetType().FullName}");
                    return this;
            }
        }
    }
}
