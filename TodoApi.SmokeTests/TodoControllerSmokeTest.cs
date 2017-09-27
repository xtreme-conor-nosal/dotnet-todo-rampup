using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TodoApi.Models;
using Xunit;

namespace TodoApi.Tests.Controllers
{
    public class TodoControllerSmokeTest
    {

        private HttpClient _client;
        
        public TodoControllerSmokeTest()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var baseUrl = configuration["Endpoint"];
            
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
        }

        [Fact]
        public async void SmokeTest()
        {
            // Create
            var requestData1 = new TodoItem();
            requestData1.Name = "New1";
            requestData1.IsComplete = false;
            var response = await _client.PostAsync("/api/todo", new StringContent(JsonConvert.SerializeObject(requestData1), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var newId1 = Convert.ToInt64(response.Headers.Location.Segments[response.Headers.Location.Segments.Length - 1]);
            requestData1.Id = newId1;
            
            var requestData2 = new TodoItem();
            requestData2.Name = "New2";
            requestData2.IsComplete = true;
            response = await _client.PostAsync("/api/todo", new StringContent(JsonConvert.SerializeObject(requestData2), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var newId2 = Convert.ToInt64(response.Headers.Location.Segments[response.Headers.Location.Segments.Length - 1]);
            requestData2.Id = newId2;
            
            // Read ID
            response = await _client.GetAsync($"/api/todo/{newId1}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var note = JsonConvert.DeserializeObject<TodoItem>(responseString);

            Assert.Equal(requestData1, note);
            
            response = await _client.GetAsync($"/api/todo/{newId2}");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            note = JsonConvert.DeserializeObject<TodoItem>(responseString);

            Assert.Equal(requestData2, note);
            
            // Read All
            response = await _client.GetAsync("/api/todo");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var notes = JsonConvert.DeserializeObject<TodoItem[]>(responseString);
            
            Assert.Contains(requestData1, notes);
            Assert.Contains(requestData2, notes);
         
            // Update
            requestData1.Name = "Updated1";
            requestData1.IsComplete = true;
            response = await _client.PutAsync($"/api/todo/{newId1}", new StringContent(JsonConvert.SerializeObject(requestData1), Encoding.UTF8, "application/json"));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            
            response = await _client.GetAsync($"/api/todo/{newId1}");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            note = JsonConvert.DeserializeObject<TodoItem>(responseString);
            
            Assert.Equal(requestData1, note);

            // Delete
            response = await _client.DeleteAsync($"/api/todo/{newId1}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            
            response = await _client.DeleteAsync($"/api/todo/{newId2}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            
            response = await _client.GetAsync("/api/todo");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            notes = JsonConvert.DeserializeObject<TodoItem[]>(responseString);
            
            Assert.DoesNotContain(requestData1, notes);
            Assert.DoesNotContain(requestData2, notes);
        }
    }
}