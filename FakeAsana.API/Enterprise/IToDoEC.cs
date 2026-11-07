using Asana.Library.Model;

namespace FakeAsana.API.Enterprise
{
    public interface IToDoEC
    {
        Task<IEnumerable<ToDo>> GetAsync();

        Task<ToDo> GetByIdAsync(int id);

        Task<IEnumerable<ToDo>> GetByProjectIdAsync(int id);

        Task<ToDo> DeleteAsync(int id);

        Task<ToDo> AddOrUpdateAsync(ToDo toDo);
    }
}