using ClassLibrary;
using System;
using System.Linq;

namespace TrainingTask
{
    public static class ConsoleUtilities
    {
        public static bool TryReadInt(string question, out int result) 
              => int.TryParse(ReadString(question), out result);

        public static string ReadString(string question)
        {
            Console.WriteLine($"{question}");
            string input = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Expected more than just an empty string there, try again.");

                input = Console.ReadLine();
            };

            return input;
        }

        public static Task PickTask(TaskCollection taskList)
        {
            taskList.ForEach(t => t.ShowShort());

            // If used to check parsing to an int can be done 
            if (Int32.TryParse(ReadString("Pick a task: "), out int i))
            {
                if (taskList.Count >= i && i > 0)
                {
                    return taskList.ElementAt(i - 1);
                }
            }
            return null;
        }
    }
}
