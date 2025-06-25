using Asana.Library.Model;
using Asana.Library.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace Asana
{

    public class Program
    {


        public static void Main(string[] args)
        {
            var toDoSvc = ToDoServiceProxy.Current;
            int choiceInt;
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

                            toDoSvc.AddOrUpdate(new ToDo
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
                            toDoSvc.DisplayToDo();
                            Console.Write("ToDo to Delete: ");
                            var toDoChoice4 = int.Parse(Console.ReadLine() ?? "0");

                            var reference = toDoSvc.GetById(toDoChoice4);
                            toDoSvc.DeleteToDo(reference);
                            break;
                        case 5:
                            toDoSvc.DisplayToDo(true);
                            Console.Write("ToDo to Update: ");
                            var toDoChoice5 = int.Parse(Console.ReadLine() ?? "0");
                            var updateReference = toDoSvc.GetById(toDoChoice5);

                            if (updateReference != null)
                            {
                                Console.Write("Name: ");
                                updateReference.Name = Console.ReadLine();
                                Console.Write("Description: ");
                                updateReference.Description = Console.ReadLine();

                            }
                            toDoSvc.AddOrUpdate(updateReference);
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