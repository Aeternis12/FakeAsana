using Asana.Library.Model;

namespace FakeAsana.API.Database
{
    public interface IDataBaseService
    {
        Task<List<Project>> GetProjectsAsync(bool expand = false);

        Task<List<ToDo>> GetToDosAsync();

        Task<Project> AddOrUpdateProjectAsync(Project project);

        Task<ToDo> AddOrUpdateToDoAsync(ToDo toDo);

        Task DeleteProjectAsync(Project project);

        Task DeleteToDoAsync(ToDo toDo);
    }
}