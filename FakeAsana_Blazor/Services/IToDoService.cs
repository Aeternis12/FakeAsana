using Asana.Library.Model;
using Newtonsoft.Json;

namespace FakeAsana_Blazor.Services
{
    public interface IToDoService
    {
        public Task<List<ToDo>> GetToDos();
        public Task<ToDo?> GetById(int id);
        public Task<List<ToDo>> GetByProjectId(int id);
        public Task<ToDo?> AddOrUpdate(ToDo? ToDo);
        public Task DeleteToDo(int id);
        
    }

}
