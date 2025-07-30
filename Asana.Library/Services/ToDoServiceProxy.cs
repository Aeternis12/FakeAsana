using Asana.Library.Model;
using Asana.Library.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public class ToDoServiceProxy
    {


        private readonly WebRequestHandler _webRequestHandler = new WebRequestHandler();

        public ToDoServiceProxy() { }

        //Singleton instance for ToDoServiceProxy
        private static object _lock = new object();
        private static ToDoServiceProxy? instance;
        public static ToDoServiceProxy Current
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new ToDoServiceProxy();
                    }
                }

                return instance;
            }
        }

        //Gets the list of ToDos from the API
        public async Task<List<ToDo>> GetToDos()
        {
            var todoData = await _webRequestHandler.Get("/ToDo");
            return JsonConvert.DeserializeObject<List<ToDo>>(todoData) ?? new List<ToDo>();
        }

        //Gets a specific ToDo by ID from the API
        public async Task<ToDo?> GetById(int id)
        {
            var toDdata = await _webRequestHandler.Get($"/ToDo/{id}");
            return JsonConvert.DeserializeObject<ToDo>(toDdata);
        }

        //Gets a list of ToDos by Project ID from the API
        public async Task<List<ToDo>> GetByProjectId(int id)
        {
            var toDoData = await _webRequestHandler.Get($"/ToDo/Project/{id}");
            return JsonConvert.DeserializeObject<List<ToDo>>(toDoData) ?? new List<ToDo>();
        }

        //Adds or updates a ToDo by sending it to the API
        public async Task<ToDo?> AddOrUpdate(ToDo? ToDo)
        {
            var toDoData = await _webRequestHandler.Post("/ToDo", ToDo);
            return JsonConvert.DeserializeObject<ToDo>(toDoData);
        }

        //Deletes a ToDo by ID by calling the API
        public async Task DeleteToDo(int id)
        {
            await _webRequestHandler.Delete($"/ToDo/{id}");
        }
    }
}
