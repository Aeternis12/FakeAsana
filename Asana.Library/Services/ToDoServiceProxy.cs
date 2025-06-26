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

        private List<ToDo> _toDoList;
        public List<ToDo> ToDos
        {
            get
            {
                return _toDoList.Take(100).ToList();
            }
            private set
            {
                if(value != _toDoList)
                {
                    _toDoList = value;
                }
            }
        }

        private ToDoServiceProxy() 
        {
            ToDos = new List<ToDo>()
            {
                new ToDo { Id = 1, Name = "Task 1", Description = "My Task 1", IsCompleted = true}, 
                new ToDo { Id = 2, Name = "Task 2", Description = "My Task 2", IsCompleted = false},
                new ToDo { Id = 3, Name = "Task 3", Description = "My Task 3", IsCompleted = true },
                new ToDo { Id = 4, Name = "Task 4", Description = "My Task 4", IsCompleted = false },
                new ToDo { Id = 5, Name = "Task 5", Description = "My Task 5", IsCompleted = true },
            };
        }

        private static ToDoServiceProxy? instance;

        private int nextKey
        {
            get
            {
                if (ToDos.Any())
                {
                    return ToDos.Select(t => t.Id).Max() + 1;
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
            return ToDos.FirstOrDefault(t => t.Id == id);
        }

        public void AddOrUpdate(ToDo? toDo)
        {
            if(toDo != null && toDo.Id == 0)
            {
                toDo.Id = nextKey;
                ToDos.Add(toDo);
            }
        }

        public void DisplayToDo(bool isShowComplete = false)
        {
            if (isShowComplete)
            {
                ToDos.ForEach(Console.WriteLine);
            }
            else
            {
                ToDos.Where(t => !(t?.IsCompleted ?? false))
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
                ToDos.Remove(toDo);
        }
    }
}
