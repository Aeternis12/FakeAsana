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

        public ToDo? GetById(int id)
        {
            return toDos.FirstOrDefault(t => t.Id == id);
        }

        public void AddOrUpdate(ToDo? toDo)
        {
            if(toDo != null && toDo.Id == 0)
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

        public void DeleteToDo(ToDo? toDo)
        {
           
            if (toDo == null)
            {
                return;
            }
            toDos.Remove(toDo);
        }
    }
}
