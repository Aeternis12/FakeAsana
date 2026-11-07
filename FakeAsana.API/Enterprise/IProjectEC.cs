using Asana.Library.Model;

namespace FakeAsana.API.Enterprise
{
    public interface IProjectEC
    {
        Task<IEnumerable<Project>> GetAsync(bool Expand = false);

        Task<Project> GetByIdAsync(int id);

        Task<Project> AddOrUpdateAsync(Project project);

        Task<Project> DeleteAsync(int id);
    }
}