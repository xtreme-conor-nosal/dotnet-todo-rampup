using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using TodoApi.Models;
using Xunit;

namespace TodoApi.Tests.Controllers
{
    public class TodoControllerTest
    {
        
        private TestServer _server;
        private HttpClient _client;
        
        public TodoControllerTest()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());
            _client = _server.CreateClient();
        }
        
        [Fact]
        public async void GetAll()
        {
            var response = await _client.GetAsync("/api/todo");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
    
            var notes = JsonConvert.DeserializeObject<TodoItem[]>(responseString);
            Assert.Equal(1, notes.Length);
            Assert.Equal(1, notes[0].Id);
            Assert.Equal("Item1", notes[0].Name);
        }
        
        [Fact]
        public async void Get()
        {
            var response = await _client.GetAsync("/api/todo/1");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
    
            var notes = JsonConvert.DeserializeObject<TodoItem>(responseString);
            Assert.Equal(1, notes.Id);
            Assert.Equal("Item1", notes.Name);
        }

        [Fact]
        public async void Create()
        {
            var requestData = new TodoItem();
            requestData.Name = "New";
            requestData.IsComplete = false;
            var response = await _client.PostAsync("/api/todo", new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("/api/Todo/2", response.Headers.Location.AbsolutePath);
            
            var getResponse = await _client.GetAsync(response.Headers.Location.AbsolutePath);
            getResponse.EnsureSuccessStatusCode();

            var responseString = await getResponse.Content.ReadAsStringAsync();
    
            var notes = JsonConvert.DeserializeObject<TodoItem>(responseString);
            Assert.Equal(2, notes.Id);
            Assert.Equal("New", notes.Name);
            Assert.Equal(false, notes.IsComplete);
        }

        [Fact]
        public async void Put()
        {
            var requestData = new TodoItem();
            requestData.Id = 1;
            requestData.Name = "New";
            requestData.IsComplete = true;
            var response = await _client.PutAsync("/api/todo/1", new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            
            var getResponse = await _client.GetAsync("/api/todo/1");
            getResponse.EnsureSuccessStatusCode();

            var responseString = await getResponse.Content.ReadAsStringAsync();
    
            var notes = JsonConvert.DeserializeObject<TodoItem>(responseString);
            Assert.Equal(1, notes.Id);
            Assert.Equal("New", notes.Name);
            Assert.Equal(true, notes.IsComplete);
        }

        [Fact]
        public async void Delete()
        {
            var response = await _client.DeleteAsync("/api/todo/1");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            
            var getResponse = await _client.GetAsync("/api/todo");
            getResponse.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
    
            var notes = JsonConvert.DeserializeObject<TodoItem[]>(responseString);
            Assert.Null(notes);
        }
    }
}