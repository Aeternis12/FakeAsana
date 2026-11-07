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
        private readonly WebRequestHandler _webRequestHandler;

        public ToDoServiceProxy(WebRequestHandler webRequestHandler)
        {
            _webRequestHandler = webRequestHandler;
            _toDos = new List<ToDo>();

        }

        private List<ToDo> _toDos = new List<ToDo>();

        public List<ToDo> ToDos { get; private set; } = new List<ToDo>();

        public async Task<List<ToDo>> GetToDos()
        {
            var todoData = await _webRequestHandler.Get("/ToDo");
            _toDos = JsonConvert.DeserializeObject<List<ToDo>>(todoData) ?? new List<ToDo>();
            return _toDos;
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
