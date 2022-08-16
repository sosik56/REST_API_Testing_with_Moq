using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TodoApi;
using TodoApi.Models;
using ToDoApi.Moq.UtilityClasses;

namespace ToDoApi.Moq.Tests
{
    public class ScenarioOfUserBehaivor
    {
        private readonly TodoItem specialItem = new TodoItem { Id = 321, Name = "User Creating Task", IsComplete = false };
        private readonly TodoItem specialItemChange = new TodoItem { Id = 321, Name = "User Created Task", IsComplete = true };

        [Test]
        public async Task UserCreat_Change_And_Delet_Task()
        {            
            WebApplicationFactory<Startup> webHost = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { });
            HttpClient httpClient = webHost.CreateClient();

            //Create
            HttpResponseMessage responseFromPost = await httpClient.PostAsJsonAsync("api/Todo",
                                             specialItem);
            Assert.AreEqual(HttpStatusCode.Created, responseFromPost.StatusCode);

            //Change
            HttpResponseMessage responseFromPut = await httpClient.PutAsJsonAsync($"api/Todo/{specialItem.Id}",
                specialItemChange);
            Assert.AreEqual(HttpStatusCode.NoContent, responseFromPut.StatusCode);


            //Get Change Item
            HttpResponseMessage response = await httpClient.GetAsync($"api/Todo/{specialItem.Id}");
            string resault = response.Content.ReadAsStringAsync().Result;
            string nameFromRespons = DeSerializeClass.GetValueFromJsonString(resault, "name");
            string isCompleteFromRespons = DeSerializeClass.GetValueFromJsonString(resault, "isComplete");
            Assert.Multiple(() =>
            {
                Assert.AreEqual(nameFromRespons, specialItemChange.Name);
                Assert.AreEqual(bool.Parse(isCompleteFromRespons), specialItemChange.IsComplete);
            });

            //Delet Item
            HttpResponseMessage responseFromDelet = await httpClient.DeleteAsync($"api/Todo/{specialItemChange.Id}");
            Assert.AreEqual(HttpStatusCode.NoContent, responseFromDelet.StatusCode);
        }
    }
}
