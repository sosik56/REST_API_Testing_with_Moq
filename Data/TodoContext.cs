using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext (DbContextOptions<TodoContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        public static void SeedData(TodoContext context)
        {           

            if (context.TodoItems.FirstOrDefaultAsync(x => x.Id == 41).Result == null)
            {
                var listOfItem = new List<TodoItem>()
                {
                    new TodoItem
                    {
                    Id= 41,
                    IsComplete = false,
                    Name = "Creat test"
                    }
                };
                context.TodoItems.AddRange(listOfItem);
                context.SaveChanges();
            }
        }
    }
}
