using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoApi.Data;
using TodoApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace TodoApi.Repository
{
    public class TodoService : IRepository<TodoItem, long>
    {
        private readonly TodoContext _context;

        public TodoService(TodoContext context) => _context = context;

        public async Task<ActionResult<TodoItem>> Create(TodoItem entity)
        {            
            await _context.AddAsync(entity);
            return entity;
        }        

        public async Task<ActionResult<TodoItem>> Delete(long id)
        {
            var item = await _context.TodoItems.FindAsync(id);
            if (item != null)
            {
                _context.TodoItems.Remove(item);
            }

            return item;
        }

        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<ActionResult<TodoItem>> GetById(long id)
        {
            if (IsExist(id))
            {
                return await _context.TodoItems.FindAsync(id);
            }
            return null;
        }

        public async Task<ActionResult<TodoItem>> Update(TodoItem item, long id)
        {
            _context.Entry(item).State = EntityState.Modified;
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public bool IsExist(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
