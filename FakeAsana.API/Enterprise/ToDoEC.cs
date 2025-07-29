using Api.ToDoApplication.Persistence;
using Asana.API.Database;
using Asana.Library.Model;

namespace Asana.API.Enterprise
{
    public class ToDoEC
    {
        public ToDoEC()
        {

        }

        public IEnumerable<ToDo> GetToDos()
        {
            return Filebase.Current.ToDos.Take(100);
        }

        public ToDo? GetById(int id)
        {
            return GetToDos().FirstOrDefault(t => t.Id == id);
        }

        public ToDo? GetByProjectID(int id)
        {
            return GetToDos().FirstOrDefault(t => t.ProjectId == id);
        }

        public ToDo? Delete(int id)
        {
            var toDoToDelete = GetById(id);
            return toDoToDelete;
        }

        public ToDo? AddOrUpdate(ToDo? toDo)
        {
            Filebase.Current.AddOrUpdateToDo(toDo);
            return toDo;
        }

        public IEnumerable<ToDo> GetByProjectId(int id)
        {
            return GetToDos().Where(t => t.ProjectId == id);
        }
    }
}

