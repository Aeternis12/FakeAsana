using Asana.Library.Model;
using Asana.Library.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeAsana_Blazor.Services
{
    public class ToDoServiceProxy : IToDoService
    {
        private readonly WebRequestHandler _webRequestHandler = new WebRequestHandler();

        public ToDoServiceProxy() { }

        public async Task<List<ToDo>> GetToDos()
        {
            var todoData = await _webRequestHandler.Get("/ToDo");
            return JsonConvert.DeserializeObject<List<ToDo>>(todoData) ?? new List<ToDo>();
        }

        public async Task<ToDo?> GetById(int id)
        {
            var toDdata = await _webRequestHandler.Get($"/ToDo/{id}");
            return JsonConvert.DeserializeObject<ToDo>(toDdata);
        }

        public async Task<List<ToDo>> GetByProjectId(int id)
        {
            var toDoData = await _webRequestHandler.Get($"/ToDo/Project/{id}");
            return JsonConvert.DeserializeObject<List<ToDo>>(toDoData) ?? new List<ToDo>();
        }

        public async Task<ToDo?> AddOrUpdate(ToDo? ToDo)
        {
            var toDoData = await _webRequestHandler.Post("/ToDo", ToDo);
            return JsonConvert.DeserializeObject<ToDo>(toDoData);
        }

        public async Task DeleteToDo(int id)
        {
            await _webRequestHandler.Delete($"/ToDo/{id}");
        }
    }
}
