
using Asana.Library.Model;
using System;

namespace Asana
{

    public class Program
    {
        static List<Project> projects = new List<Project>();
        static List<ToDo> toDos = new List<ToDo>();
        static int totalToDoCount = 0;
        static int totalProjectCount = 0;

        public static void Main(string[] args)
        {
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
                            CreateToDo();
                            break;
                        case 2:
                            ListAllToDo();
                            break;
                        case 3:
                            ListAllOutstandingToDo();
                            break;
                        case 4:
                            DeleteToDo();
                            break;
                        case 5:
                            UpdateToDo();
                            break;
                        case 6:
                            CreateProject();
                            break;
                        case 7:
                            ListAllProject();
                            break;
                        case 8:
                            ListAllToDosInProject();
                            break;
                        case 9:
                            DeleteProject();
                            break;
                        case 10:
                            UpdateProject();
                            break;
                        case 11:
                            MarkToDoAsCompleted();
                            break;
                        case 0:
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

            } while (choiceInt != 0);
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
            Console.WriteLine(new string('-', 35));
            Console.WriteLine("| 6.  Create a Project            |");
            Console.WriteLine("| 7.  List all Projects           |");
            Console.WriteLine("| 8.  List all ToDos in a Project |");
            Console.WriteLine("| 9.  Delete a Project            |");
            Console.WriteLine("| 10. Update a Project            |");
            Console.WriteLine("| 11. Mark a ToDo as completed    |");
            Console.WriteLine("| 0.  Exit                        |");
            Console.WriteLine(new string('-', 35));
        }
        public static void CreateToDo()
        {
            Console.Write("Name:");
            var name = Console.ReadLine();
            Console.Write("Description:");
            var description = Console.ReadLine();
            Console.Write("Priority (1-5):");
            var priority = int.Parse(Console.ReadLine() ?? "1");
            ListAllProject();
            Console.Write("Which Project does this ToDo belong to? (0 for none):");
            var projectId = int.Parse(Console.ReadLine() ?? "0");
            var chosenProject = projects.FirstOrDefault(t => t.Id == projectId);

            var newToDo = new ToDo
            {
                Name = name,
                Description = description,
                IsCompleted = false,
                Id = ++totalToDoCount,
                Priority = priority,
                ProjectId = projectId
            };

            if (chosenProject == null || projectId == 0)
                toDos.Add(newToDo);
            else
            {
                chosenProject.ToDos.Add(newToDo);
                toDos.Add(newToDo);
            }
        }
        public static void ListAllToDo()
        {
            toDos.ForEach(Console.WriteLine);
        }
        public static void ListAllOutstandingToDo()
        {
            toDos.Where(t => (t != null) && !(t?.IsCompleted ?? false))
                                .ToList()
                                .ForEach(Console.WriteLine);
        }
        public static void DeleteToDo()
        {
            ListAllToDo();
            Console.Write("ToDo to Delete: ");
            var toDoChoice = int.Parse(Console.ReadLine() ?? "0");

            var toDoReference = toDos.FirstOrDefault(t => t.Id == toDoChoice);
            if (toDoReference != null)
            {
                if (toDoReference.ProjectId != 0)
                {
                    var projectBelong = projects.FirstOrDefault(p => p.Id == toDoReference.ProjectId);
                    if (projectBelong != null)
                    {
                        projectBelong.ToDos.Remove(toDoReference);
                    }
                }
                toDos.Remove(toDoReference);
            }
        }
        public static void UpdateToDo()
        {
            ListAllToDo();
            Console.Write("ToDo to Update: ");
            var toDoChoice = int.Parse(Console.ReadLine() ?? "0");

            var toDoToUpdate = toDos.FirstOrDefault(t => t.Id == toDoChoice);
            if (toDoToUpdate != null)
            {
                Console.Write("Name:");
                toDoToUpdate.Name = Console.ReadLine();
                Console.Write("Description:");
                toDoToUpdate.Description = Console.ReadLine();
                Console.Write("Priority (1-5):");
                toDoToUpdate.Priority = int.Parse(Console.ReadLine() ?? "1");
            }
        }
        public static void CreateProject()
        {
            Console.Write("Project Name:");
            var projectName = Console.ReadLine();
            Console.Write("Project Description:");
            var projectDescription = Console.ReadLine();

            projects.Add(new Project
            {
                Name = projectName,
                Description = projectDescription,
                Id = ++totalProjectCount
            });
        }
        public static void ListAllProject()
        {
            projects.ForEach(Console.WriteLine);
        }
        public static void ListAllToDosInProject()
        {
            ListAllProject();
            Console.Write("Which Project Id to list ToDos: ");
            var projectIdToList = int.Parse(Console.ReadLine() ?? "0");
            var projectToList = projects.FirstOrDefault(p => p.Id == projectIdToList);
            if (projectToList != null && projectToList.ToDos != null)
            {
                Console.WriteLine($"ToDos for Project: {projectToList.Name}");
                Console.WriteLine(new string('-', 50));
                projectToList.ToDos.Where(t => (t != null) && !(t?.IsCompleted ?? false))
                                .ToList()
                                .ForEach(Console.WriteLine);
            }
            else
            {
                Console.WriteLine("Project not found or has no ToDos.");
            }
        }
        public static void DeleteProject()
        {
            ListAllProject();
            Console.Write("Project to Delete: ");
            var projectChoice = int.Parse(Console.ReadLine() ?? "0");

            var projectReference = projects.FirstOrDefault(t => t.Id == projectChoice);
            if (projectReference != null)
            {
                projects.Remove(projectReference);
            }
        }
        public static void UpdateProject()
        {
            ListAllProject();
            Console.Write("Project to Update: ");
            var projectChoice = int.Parse(Console.ReadLine() ?? "0");

            var projectToUpdate = projects.FirstOrDefault(t => t.Id == projectChoice);
            if (projectToUpdate != null)
            {
                Console.Write("Name:");
                projectToUpdate.Name = Console.ReadLine();
                Console.Write("Description:");
                projectToUpdate.Description = Console.ReadLine();
            }
        }
        public static void MarkToDoAsCompleted()
        {
            ListAllToDo();
            Console.Write("Which ToDo is complete: ");
            var toDoCompleteId = int.Parse(Console.ReadLine() ?? "0");
            var toDoToComplete = toDos.FirstOrDefault(t => t.Id == toDoCompleteId);
            if (toDoToComplete != null)
            {
                toDoToComplete.IsCompleted = true;
                Console.WriteLine($"ToDo [{toDoToComplete.Id}] marked as completed.");
            }
        }
        
    }
}


