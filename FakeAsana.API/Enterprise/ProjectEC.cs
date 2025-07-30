using Api.ToDoApplication.Persistence;
using Asana.Library.Model;

namespace Asana.API.Enterprise
{
    public class ProjectEC
    {
        //Gets the list of projects from file, optionally expanding the ToDos for each project
        public IEnumerable<Project>? Get(bool Expand = false)
        {
            return Filebase.Current.GetProjects(Expand)?.Take(100);
        }

        //Gets the a specific project by its Id from file, including its ToDos
        public Project? GetById(int id)
        {
            return Filebase.Current.GetProjects(true)?.FirstOrDefault(p => p.Id == id);
        }

        //Adds or updates a project in the file system
        public Project? AddOrUpdate(Project? project)
        {
            if(project == null)
            {
                return project;
            }
            Filebase.Current.AddOrUpdateProject(project);
            return project;
        }

        //Deletes a project by its Id from the file system
        public Project? Delete(int id)
        {
            var projectToDelete = GetById(id);
            if (projectToDelete != null)
            {
                Filebase.Current.DeleteProject(projectToDelete);
            }
            return projectToDelete;
        }
    }
}
