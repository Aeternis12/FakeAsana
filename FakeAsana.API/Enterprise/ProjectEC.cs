using Asana.Library.Model;
using FakeAsana.API.Database;

namespace FakeAsana.API.Enterprise
{
    public class ProjectEC : IProjectEC
    {
        private readonly IDataBaseService _db;

        public ProjectEC(IDataBaseService db)
        {
            _db = db;
        }

        //Gets the list of projects from file, optionally expanding the ToDos for each project
        public async Task<IEnumerable<Project>> GetAsync(bool Expand = false)
        {
            var projects = await _db.GetProjectsAsync(Expand);
            return projects.Take(100);
        }

        //Gets the a specific project by its Id from file, including its ToDos
        public async Task<Project> GetByIdAsync(int id)
        {
            var project = await _db.GetProjectsAsync(true);
            return project.FirstOrDefault(p => p.Id == id);
        }

        //Adds or updates a project in the file system
        public async Task<Project> AddOrUpdateAsync(Project? project)
        {
            if (project != null)
            {
                await _db.AddOrUpdateProjectAsync(project);
            }
            return project;
        }

        //Deletes a project by its Id from the file system
        public async Task<Project> DeleteAsync(int id)
        {
            var projectToDelete = await GetByIdAsync(id);
            if (projectToDelete != null)
            {
                await _db.DeleteProjectAsync(projectToDelete);
            }
            return projectToDelete;
        }
    }
}