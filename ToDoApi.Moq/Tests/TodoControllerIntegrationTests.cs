using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TodoApi;
using TodoApi.Models;
using ToDoApi.Moq.MockData;
using ToDoApi.Moq.UtilityClasses;

namespace ToDoApi.Moq.Tests
{
    
    public class TodoControllerIntegrationTests
    {
        private readonly long idOfDosentExistingItem = 123415;
        private readonly long idOfExistingItem = 41;

        [Test]
        public async Task PostTheTodoItem_ShouldRetrun_Create()
        {
            //Arange
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { });
            HttpClient httpClient = webHost.CreateClient();

            //Act
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/Todo",
                                             TodoMockData.GetItem());

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            webHost.Dispose();
        }

        [Test]
        public async Task GetTodoItem_ShouldReturnAllItems()
        {
            //Arange
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { });
            HttpClient httpClient = webHost.CreateClient();

            //Act
            HttpResponseMessage response = await httpClient.GetAsync("api/Todo");
            var countOfRespons = response.Content.ReadAsStringAsync().Result;

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            webHost.Dispose();
        }

        [Test]
        public async Task GetTodoItem_ShouldReturnToDoItem_WhenItemExist()
        {
            //Arange            
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { });
            HttpClient httpClient = webHost.CreateClient();

            //Act            
            HttpResponseMessage response = await httpClient.GetAsync($"api/Todo/{idOfExistingItem}");
            string resault = response.Content.ReadAsStringAsync().Result;
            long idFromRespons =  long.Parse(DeSerializeClass.GetValueFromJsonString(resault, "id"));

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.AreEqual(idOfExistingItem, idFromRespons);
            });            
            webHost.Dispose();
        }

        [Test]
        public async Task GetTodoItem_ShouldReturnNothing_WhenItemDosentExist()
        {
            //Arange            
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { });
            HttpClient httpClient = webHost.CreateClient();

            //Act            
            HttpResponseMessage response = await httpClient.GetAsync($"api/Todo/{idOfDosentExistingItem}");
             
            //Assert            
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);             
            webHost.Dispose();
        }

        [Test]
        public async Task DeleteTodoItem_NoContent_WhenItemExist()
        {
            //Arange           
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { });
            HttpClient httpClient = webHost.CreateClient();

            //Act            
            HttpResponseMessage response = await httpClient.DeleteAsync($"api/Todo/{idOfExistingItem}");
            
            //Assert            
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            webHost.Dispose();
        }

        [Test]
        public async Task DeleteTodoItem_ShouldReturnNothing_WhenItemDosentExist()
        {
            //Arange            
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { });
            HttpClient httpClient = webHost.CreateClient();

            //Act            
            HttpResponseMessage response = await httpClient.DeleteAsync($"api/Todo/{idOfDosentExistingItem}");
          
            //Assert            
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            webHost.Dispose();
        }

        [Test]
        public async Task PutTodoItem_ShouldReturn204_WhenIdItemExist()
        {
            //Arange            
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { });
            HttpClient httpClient = webHost.CreateClient();

            //Act            
            HttpResponseMessage response = await httpClient.PutAsJsonAsync($"api/Todo/{idOfExistingItem}", 
                new TodoItem { Id = idOfExistingItem, IsComplete=false, Name = "New Task"});

            //Assert            
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
            webHost.Dispose();
        }

        [Test]
        public async Task PutTodoItem_ShouldReturn400_WhenItemDosentExist()
        {
            //Arange            
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { });
            HttpClient httpClient = webHost.CreateClient();

            //Act            
            HttpResponseMessage response = await httpClient.PutAsJsonAsync($"api/Todo/{idOfDosentExistingItem}",
                new TodoItem { Id = idOfDosentExistingItem, IsComplete = false, Name = "New Task" });

            //Assert            
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            webHost.Dispose();
        }
    }
}
