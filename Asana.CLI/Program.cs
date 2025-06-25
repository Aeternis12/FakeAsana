using Asana.Library.Model;
using Asana.Library.Services;
using System;

namespace Asana
{

    public class Program
    {


        public static void Main(string[] args)
        {
            var toDoSvc = ToDoServiceProxy.Current;
            int choiceInt;
            var itemCount = 0;
            do
            {
                PrintMenu();
                var choice = Console.ReadLine() ?? "0";

                if (int.TryParse(choice, out choiceInt))
                {
                    switch (choiceInt)
                    {
                        case 1:
                            Console.Write("Name: ");
                            var name = Console.ReadLine();
                            Console.Write("Description: ");
                            var description = Console.ReadLine();

                            toDoSvc.CreateToDo(new ToDo
                            {
                                Name = name,
                                Description = description,
                                IsCompleted = false,
                                Id = 0
                            });
                            break;
                        case 2:
                            toDoSvc.DisplayToDo(true);
                            break;
                        case 3:
                            toDoSvc.DisplayToDo();
                            break;
                        case 4:
                            toDoSvc.DeleteToDo();
                            break;
                        case 5:
                            toDoSvc.UpdateToDo();
                            break;
                        case 6:
                            Console.WriteLine("Exiting the application...");
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

            } while (choiceInt != 6);
        }

        public static void PrintMenu()
        {
            Console.WriteLine("Choose a menu option:");
            Console.WriteLine(new string('-', 35));
            Console.WriteLine("| 1.  Create a ToDo               |");
            Console.WriteLine("| 2.  List all TodDos             |");
            Console.WriteLine("| 3.  List all outstanding TodDos |");
            Console.WriteLine("| 4.  Delete a ToDo               |");
            Console.WriteLine("| 5.  Update a ToDo               |");
            Console.WriteLine("| 6.  Exit                        |");
            Console.WriteLine(new string('-', 35));
        }
    }
}