using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models;
using Xunit;

namespace TodoApi.unit
{
    public class TodoServiceTest
    {
        private ITodoContext context;
        private Mock<ITodoContext> mockContext;
        
        
        private List<TodoItem> items;
        private TodoService subject;
        
        public TodoServiceTest()
        {
            context = Mock.Of<ITodoContext>();
            mockContext = Mock.Get(context);
            
            items = new List<TodoItem>(new[]
                {
                    new TodoItem {Id = 1, Name = "1", IsComplete = true},
                    new TodoItem {Id = 2, Name = "2", IsComplete = false}
                }
            );
            mockContext.Setup(s => s.GetAll()).Returns(items);
            mockContext.Setup(s => s.Count()).Returns(2);
        }
        
        [Fact]
        public void TodoServiceInitializesDatabaseWithOneItem()
        {
            mockContext.Setup(s => s.Count()).Returns(0);
            subject = new TodoService(mockContext.Object);
            
            mockContext.Verify(d => d.EnsureCreated());
            mockContext.Verify(s => s.Add(new TodoItem { Name = "Item1" }));
        }

        [Fact]

        public void test()
        {
            Assert.Equal(new TodoItem { Name = "Item1" }, new TodoItem { Name = "Item1" });
        }
        
        [Fact]
        public void TodoServiceDoesNotReaddFirstItem()
        {
            subject = new TodoService(mockContext.Object);
            
            mockContext.Verify(d => d.EnsureCreated());
            mockContext.Verify(s => s.Add(new TodoItem { Name = "Item1" }), Times.Never);
        }

        [Fact]
        public void GetAllReturnsFullSet()
        {
            subject = new TodoService(mockContext.Object);
            Assert.Equal(items, subject.GetAll());
        }

        [Fact]
        public void GetById_existingItem_returnsItem()
        {
            subject = new TodoService(mockContext.Object);
            mockContext.Setup(s => s.GetById(1)).Returns(items[0]);
            Assert.Equal(items[0], subject.GetById(1));
        }

        [Fact]
        public void GetById_invalidId_returnsNull()
        {
            subject = new TodoService(mockContext.Object);
            mockContext.Setup(s => s.GetById(4)).Returns((TodoItem)null);
            Assert.Null(subject.GetById(4));
        }

        [Fact]
        public void Create_addsItemAndReturnsWithId()
        {
            subject = new TodoService(mockContext.Object);
            TodoItem todoItem = new TodoItem { Name = "New", IsComplete = false };
            TodoItem saved = subject.Create(todoItem);
            mockContext.Verify(s => s.Add(todoItem));
            
            Assert.Equal(todoItem, saved);
        }

        [Fact]
        public void Update_updatesItem()
        {
            subject = new TodoService(mockContext.Object);
            TodoItem todoItem = new TodoItem { Id = 2, Name = "New", IsComplete = true };
            TodoItem loadedItem = new TodoItem { Id = 2 };
            mockContext.Setup(s => s.GetById(2)).Returns(loadedItem);
            subject.Update(todoItem);
            mockContext.Verify(s => s.Update(loadedItem));
            
            Assert.Equal("New", loadedItem.Name);
            Assert.Equal(true, loadedItem.IsComplete);
        }

        [Fact]
        public void DeleteById_deletesItem()
        {
            subject = new TodoService(mockContext.Object);
            TodoItem loadedItem = new TodoItem { Id = 2 };
            mockContext.Setup(s => s.GetById(2)).Returns(loadedItem);
            subject.DeleteById(2);
            mockContext.Verify(s => s.Remove(loadedItem));
        }
    }
}