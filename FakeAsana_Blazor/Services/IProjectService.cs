using Asana.Library.Model;

namespace FakeAsana_Blazor.Services
{
    public interface IProjectService
    {
        Task<List<Project>> GetProjects();
        Task<Project?> GetById(int id);
        Task<Project?> AddOrUpdate(Project? project);
        Task DeleteProject(int id);
    }
}