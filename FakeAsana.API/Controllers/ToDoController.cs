using FakeAsana.API.Enterprise;
using Asana.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace FakeAsana.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController
    {
        private readonly IToDoEC _toDoEC;

        public ToDoController(IToDoEC toDo)
        {
            _toDoEC = toDo;
        }

        //Gets all ToDos from the EC, which reads from the file system
        [HttpGet]
        public async Task<IEnumerable<ToDo>> GetAsync()
        {
            var result = await _toDoEC.GetAsync();
            return result;
        }

        //Gets a ToDo by its Id from the EC, which reads from the file system
        [HttpGet("{id}")]
        public async Task<ToDo> GetByIdAsync(int id)
        {
            var result = await _toDoEC.GetByIdAsync(id);
            return result;
        }

        //Gets a ToDo by its ProjectId from the EC, which reads from the file system
        [HttpDelete("{id}")]
        public async Task<ToDo> DeleteAsync(int id)
        {
            var result = await _toDoEC.DeleteAsync(id);
            return result;
        }

        //Adds or updates a ToDo in the EC, which writes to the file system
        [HttpPost]
        public async Task<ToDo> AddOrUpdateAsync([FromBody] ToDo? toDo)
        {
            var result = await _toDoEC.AddOrUpdateAsync(toDo);
            return result;
        }

        //Gets all ToDos for a specific project by its Id from the EC, which reads from the file system
        [HttpGet("Project/{id}")]
        public async Task<IEnumerable<ToDo>> GetByProjectIdAsync(int id)
        {
            var result = await _toDoEC.GetByProjectIdAsync(id);
            return result;
        }
    }
}