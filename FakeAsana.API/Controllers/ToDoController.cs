using Asana.API.Enterprise;
using Asana.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace Asana.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        //Gets all ToDos from the EC, which reads from the file system
        [HttpGet]
        public IEnumerable<ToDo> Get()
        {
            return new ToDoEC().GetToDos();
        }

        //Gets a ToDo by its Id from the EC, which reads from the file system
        [HttpGet("{id}")]       
        public ToDo? GetById(int id)
        {
            return new ToDoEC().GetById(id);
        }

        //Gets a ToDo by its ProjectId from the EC, which reads from the file system
        [HttpDelete("{id}")]
        public ToDo? Delete(int id)
        {
            return new ToDoEC().Delete(id);
        }

        //Adds or updates a ToDo in the EC, which writes to the file system
        [HttpPost]
        public ToDo? AddOrUpdate([FromBody] ToDo? toDo)
        {
            return new ToDoEC().AddOrUpdate(toDo);
        }

        //Gets all ToDos for a specific project by its Id from the EC, which reads from the file system
        [HttpGet("Project/{id}")]
        public IEnumerable<ToDo> GetByProjectId(int id)
        {
            return new ToDoEC().GetByProjectId(id);
        }
    }
}
