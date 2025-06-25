using Asana.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public class ToDoServiceProxy
    {
        public List<ToDo> toDos;

        private ToDoServiceProxy() 
        {
            toDos = new List<ToDo>();
        }

        private static ToDoServiceProxy? instance;

        private int nextKey
        {
            get
            {
                if (toDos.Any())
                {
                    return toDos.Select(t => t.Id).Max() + 1;
                }
                return 1;
            }
        }

        public static ToDoServiceProxy Current
        {
            get
            {
                if(instance == null)
                {
                    instance = new ToDoServiceProxy();
                }

                return instance;
            }
        }
        public void CreateToDo(ToDo toDo)
        {
            if(toDo.Id == 0)
            {
                toDo.Id = nextKey;
                toDos.Add(toDo);
            }
        }

        public void DisplayToDo(bool isShowComplete = false)
        {
            if (isShowComplete)
            {
                toDos.ForEach(Console.WriteLine);
            }
            else
            {
                toDos.Where(t => !(t?.IsCompleted ?? false))
                     .ToList()
                     .ForEach(Console.WriteLine);
            }
        }

        public void DeleteToDo()
        {
            toDos.ForEach(Console.WriteLine);
            Console.Write("ToDo to Delete: ");
            var toDoChoice = int.Parse(Console.ReadLine() ?? "0");

            var reference = toDos.FirstOrDefault(t => t.Id == toDoChoice);
            if (reference != null)
            {
                toDos.Remove(reference);
            }
        }

        public void UpdateToDo()
        {
            toDos.ForEach(Console.WriteLine);
            Console.Write("ToDo to Update: ");
            var toDoChoice = int.Parse(Console.ReadLine() ?? "0");
            var updateReference = toDos.FirstOrDefault(t => t.Id == toDoChoice);

            if (updateReference != null)
            {
                Console.Write("Name: ");
                updateReference.Name = Console.ReadLine();
                Console.Write("Description: ");
                updateReference.Description = Console.ReadLine();

            }
        }
    }
}
