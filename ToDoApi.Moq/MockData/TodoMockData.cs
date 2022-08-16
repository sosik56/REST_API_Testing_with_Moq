using System.Collections.Generic;
using TodoApi.Models;

namespace ToDoApi.Moq.MockData
{
    public static class TodoMockData
    {
        public static List<TodoItem> GetListOfItems()
        {
            return new List<TodoItem>
            {
                new TodoItem
                {
                    Id = 41,
                    IsComplete = false,
                    Name = "Creat test"
                },

                 new TodoItem
                {
                    Id = 22,
                    IsComplete = false,
                    Name = "Washe the dishes"
                },

                  new TodoItem
                {
                    Id = 13,
                    IsComplete = true,
                    Name = "Feed the cat"
                }
            };
        }

        public static TodoItem GetItem()
        {
            return new TodoItem
            {
                Id = 12,
                IsComplete = false,
                Name = "Creat test"
            };
        }
    }
}
