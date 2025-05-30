
using Asana.Library.Model;
using System;

namespace Asana
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var toDos = new List<ToDo>();
            int choiceInt; 

            do
            {
                Console.WriteLine("Choose a mneu option:");
                Console.WriteLine("1. Create a ToDo");
                Console.WriteLine("2. List all TodDos");
                Console.WriteLine("3. Exit");

                var choice = Console.ReadLine() ?? "3";

                if (int.TryParse(choice, out choiceInt))
                {
                    switch (choiceInt)
                    {
                        case 1:
                            Console.Write("Name:");
                            var name = Console.ReadLine();
                            Console.Write("Description:");
                            var description = Console.ReadLine();

                            toDos.Add(new ToDo { Name = name, Description = description });
                            break;
                        case 2:
                            toDos.ForEach(Console.WriteLine);
                            break;
                        case 3:
                            break;
                        default:
                            Console.WriteLine("ERROR: Unknown menu selection");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"ERROR: {choice} is not a valid menu selection");
                }

            } while (choiceInt != 3);
        }   
    }
}