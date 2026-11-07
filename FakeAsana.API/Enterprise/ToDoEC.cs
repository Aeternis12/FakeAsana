using Asana.Library.Model;
using FakeAsana.API.Database;

namespace FakeAsana.API.Enterprise
{
    public class ToDoEC : IToDoEC
    {
        private readonly IDataBaseService _db;

        public ToDoEC(IDataBaseService db)
        {
            _db = db;
        }

        //Gets the list of ToDos from file
        public async Task<IEnumerable<ToDo>> GetAsync()
        {
            var toDos = await _db.GetToDosAsync();
            return toDos.Take(100);
        }

        //Gets a specific ToDo by its Id from file
        public async Task<ToDo> GetByIdAsync(int id)
        {
            var toDo = await _db.GetToDosAsync();
            return toDo.FirstOrDefault(t => t.Id == id);
        }

        //Deletes a ToDo by its Id from the file system
        public async Task<ToDo> DeleteAsync(int id)
        {
            var toDoToDelete = await GetByIdAsync(id);
            if (toDoToDelete != null)
            {
                await _db.DeleteToDoAsync(toDoToDelete);
            }

            return toDoToDelete;
        }

        //Adds or updates a ToDo in the file system
        public async Task<ToDo> AddOrUpdateAsync(ToDo toDo)
        {
            if (toDo != null)
            {
                await _db.AddOrUpdateToDoAsync(toDo);
            }
            return toDo;
        }

        //Gets all ToDos for a specific project by its ProjectId
        public async Task<IEnumerable<ToDo>> GetByProjectIdAsync(int id)
        {
            var toDos = await _db.GetToDosAsync();
            return toDos.Where(t => t.ProjectId == id);
        }
    }
}