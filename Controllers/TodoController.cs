using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Repository;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class TodoController : Controller
    {
        private readonly IRepository<TodoItem, long> _todoRepository;

        public TodoController(IRepository<TodoItem, long> todoRepository)
        {
            _todoRepository = todoRepository;            
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItem()
        {           
            return  await _todoRepository.GetAll();            
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _todoRepository.GetById(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/Todo/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItem>> PutTodoItem(long id, TodoItem todoItem)
        {           
            if (!_todoRepository.IsExist(id))
            {
                return BadRequest();
            }

            await _todoRepository.Update(todoItem, id);                        
            await _todoRepository.Save();           

            return NoContent();
        }

        // POST: api/Todo
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult> PostTodoItem(TodoItem todoItem)
        {
            await _todoRepository.Create(todoItem);
            await _todoRepository.Save();            
            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
        {            
            if (!_todoRepository.IsExist(id))
            {
                return NotFound();
            }

            await _todoRepository.Delete(id);
            await _todoRepository.Save();

            return NoContent();
        }       
    }
}
