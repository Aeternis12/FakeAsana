using Api.ToDoApplication.Persistence;
using Asana.Library.Model;

namespace Asana.API.Enterprise
{
    public class ToDoEC
    {

        //Gets the list of ToDos from file
        public IEnumerable<ToDo> GetToDos()
        {
            return Filebase.Current.ToDos.Take(100);
        }

        //Gets a specific ToDo by its Id from file
        public ToDo? GetById(int id)
        {
            return GetToDos().FirstOrDefault(t => t.Id == id);
        }

        //Gets all ToDos for a specific project by its ProjectId
        public ToDo? GetByProjectID(int id)
        {
            return GetToDos().FirstOrDefault(t => t.ProjectId == id);
        }

        //Deletes a ToDo by its Id from the file system
        public ToDo? Delete(int id)
        {
            var toDoToDelete = GetById(id);
            Filebase.Current.DeleteToDo(toDoToDelete);
            return toDoToDelete;
        }

        //Adds or updates a ToDo in the file system
        public ToDo? AddOrUpdate(ToDo? toDo)
        {
            Filebase.Current.AddOrUpdateToDo(toDo);
            return toDo;
        }

        //Gets all ToDos for a specific project by its ProjectId
        public IEnumerable<ToDo> GetByProjectId(int id)
        {
            return GetToDos().Where(t => t.ProjectId == id);
        }
    }
}

