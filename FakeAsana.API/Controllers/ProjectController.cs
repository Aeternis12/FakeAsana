using Asana.Library.Model;
using Asana.API.Enterprise;
using Microsoft.AspNetCore.Mvc;

namespace Asana.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        //Gets all projects from the EC, which reads from the file system
        [HttpGet]
        public IEnumerable<Project>? Get() {
            return new ProjectEC().Get();
        }

        //Gets all projects with the expanded ToDos for each project from the EC, which reads from the file system
        [HttpGet("Expand")]
        public IEnumerable<Project>? GetExpand()
        {
            return new ProjectEC().Get(true);
        }

        //Gets a project by its Id from the EC, which reads from the file system
        [HttpGet("{id}")]
        public Project? GetById(int id)
        {
            return new ProjectEC().GetById(id);
        }

        //Deletes a project by its Id from the EC, which reads from the file system
        [HttpDelete("{id}")]
        public Project? Delete(int id)
        {
            return new ProjectEC().Delete(id);
        }

        //Adds or updates a project in the EC, which writes to the file system
        [HttpPost]
        public Project? AddOrUpdate([FromBody] Project? project)
        {
            return new ProjectEC().AddOrUpdate(project);
        }
    }
}
