using Asana.Library.Model;
using FakeAsana.API.Enterprise;
using Microsoft.AspNetCore.Mvc;

namespace FakeAsana.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController
    {
        private readonly IProjectEC _projectEC;

        public ProjectController(IProjectEC projectEC)
        {
            _projectEC = projectEC;
        }

        //Gets all projects from the EC, which reads from the file system
        [HttpGet]
        public async Task<IEnumerable<Project>> GetAsync()
        {
            var result = await _projectEC.GetAsync();
            return result;
        }

        //Gets all projects with the expanded ToDos for each project from the EC, which reads from the file system
        [HttpGet("Expand")]
        public async Task<IEnumerable<Project>> GetExpandAsync()
        {
            var result = await _projectEC.GetAsync(true);
            return result;
        }

        //Gets a project by its Id from the EC, which reads from the file system
        [HttpGet("{id}")]
        public async Task<Project> GetByIdAsync(int id)
        {
            var result = await _projectEC.GetByIdAsync(id);
            return result;
        }

        //Deletes a project by its Id from the EC, which reads from the file system
        [HttpDelete("{id}")]
        public async Task<Project> DeleteAsync(int id)
        {
            var result = await _projectEC.DeleteAsync(id);
            return result;
        }

        //Adds or updates a project in the EC, which writes to the file system
        [HttpPost]
        public async Task<Project> AddOrUpdateAsync([FromBody] Project? project)
        {
            var result = await _projectEC.AddOrUpdateAsync(project);
            return result;
        }
    }
}