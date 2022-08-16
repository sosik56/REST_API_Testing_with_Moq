using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Reflection;
using System.Threading.Tasks;
using TodoApi.Controllers;
using TodoApi.Models;
using TodoApi.Repository;
using ToDoApi.Moq.MockData;

namespace ToDoApi.Moq.Tests
{
    public class ToDoControllerUnitTests
    {
        private readonly TodoController _sut;
        private readonly Mock<IRepository<TodoItem, long>> _todoItemRepository = new Mock<IRepository<TodoItem, long>>();

        public ToDoControllerUnitTests()
        {
            _sut = new TodoController(_todoItemRepository.Object);
        }       

        [Test]
        public async Task GetTodoItem_ShouldReturnAllItems()
        {
            ///Arange
            var listOfItems = TodoMockData.GetListOfItems();
            _todoItemRepository.Setup(x => x.GetAll()).ReturnsAsync(listOfItems);         

            //Act
            var todoItemsFromRepo = await _sut.GetTodoItem();
            var valueTodo = todoItemsFromRepo.Value;
            
            var sizeField = valueTodo.GetType().GetField("_size", BindingFlags.NonPublic | BindingFlags.Instance);
            var itemsField = valueTodo.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);

            var countFromRepo = sizeField.GetValue(valueTodo);
            var itemsValue = itemsField.GetValue(valueTodo);
            TodoItem[] itemsArray = itemsValue as TodoItem[];           
                       
            ///Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(listOfItems.Count, countFromRepo);
                Assert.AreEqual(itemsArray[0].Id, listOfItems[0].Id);
                Assert.AreEqual(itemsArray[1].IsComplete, listOfItems[1].IsComplete);
                Assert.AreEqual(itemsArray[2].Name, listOfItems[2].Name);
            });
        }

        [Test]
        public async Task GetTodoItem_ShouldReturnToDoItem_WhenItemExist()
        {
            ///Arange
            var todoItem = TodoMockData.GetItem();
            _todoItemRepository.Setup(x => x.GetById(todoItem.Id)).ReturnsAsync(todoItem);

            //Act
            var todoItemFromRepo = await _sut.GetTodoItem(todoItem.Id);

            ///Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(todoItem.Id, todoItemFromRepo.Value.Id);
                Assert.AreEqual(todoItem.Name, todoItemFromRepo.Value.Name);
                Assert.AreEqual(todoItem.IsComplete, todoItemFromRepo.Value.IsComplete);
            });            
        }

        [Test]
        public async Task GetTodoItem_ShouldReturnNothing_WhenItemDosentExist()
        {
            ///Arange
            long randomId = 123;
            _todoItemRepository.Setup(x => x.GetById(It.IsAny<long>())).ReturnsAsync(() => null);

            //Act
            var todoItemFromRepo = await _sut.GetTodoItem(randomId);

            ///Assert
            Assert.Multiple(() => 
            {
                Assert.Null(todoItemFromRepo.Value);
                Assert.AreEqual(new NotFoundResult().GetType(), todoItemFromRepo.Result.GetType());
            });
        }

        [Test]
        public async Task PostTodoItem_ShouldReturn201()
        {
            ///Arange
            var todoItem = TodoMockData.GetItem();

            //Act
            var todoItemFromRepo = await _sut.PostTodoItem(todoItem);                    
            var statusCodeField = todoItemFromRepo.GetType().GetRuntimeProperty("StatusCode");
            var statusCodeFieldValue = statusCodeField.GetValue(todoItemFromRepo);

            ///Assert
            Assert.AreEqual(201 , statusCodeFieldValue);            
        }

        [Test]
        public async Task DeleteTodoItem_ShouldReturnNothing_WhenItemDosentExist()
        {
            ///Arange
            var todoItem = TodoMockData.GetItem();
            _todoItemRepository.Setup(x => x.IsExist(todoItem.Id)).Returns(false);

            //Act
            var resultFromRepo = await _sut.DeleteTodoItem(todoItem.Id);

            ///Assert
            Assert.AreEqual(new NotFoundResult().GetType(), resultFromRepo.Result.GetType());            
        }

        [Test]
        public async Task DeleteTodoItem_NoContent_WhenItemExist()
        {
            ///Arange
            var todoItem = TodoMockData.GetItem();
            _todoItemRepository.Setup(x => x.IsExist(todoItem.Id)).Returns(true);
            _todoItemRepository.Setup(x => x.GetById(todoItem.Id)).ReturnsAsync(todoItem);

            //Act
            var resultFromRepo = await _sut.DeleteTodoItem(todoItem.Id);

            ///Assert
            Assert.AreEqual(new NoContentResult().GetType(), resultFromRepo.Result.GetType());
        }

        [Test]
        public async Task PutTodoItem_ShouldReturn204_WhenIdItemExist()
        {
            ///Arange
            var todoItem = TodoMockData.GetItem();
            _todoItemRepository.Setup(x => x.GetById(todoItem.Id)).ReturnsAsync(todoItem);
            _todoItemRepository.Setup(x =>x.IsExist(todoItem.Id)).Returns(true);
            todoItem.Name = "New Name";

            //Act
            var resultFromRepo = await _sut.PutTodoItem(todoItem.Id, todoItem);

            ///Assert
            Assert.AreEqual(new NoContentResult().GetType(), resultFromRepo.Result.GetType());
        }

        [Test]
        public async Task PutTodoItem_ShouldReturn400_WhenItemDosentExist()
        {
            ///Arange
            var todoItem = TodoMockData.GetItem();
            _todoItemRepository.Setup(x => x.IsExist(It.IsAny<long>())).Returns(false);

            //Act
            var resultFromRepo = await _sut.PutTodoItem(todoItem.Id, todoItem);

            ///Assert
            Assert.AreEqual(new BadRequestResult().GetType(), resultFromRepo.Result.GetType());
        }        
    }
}