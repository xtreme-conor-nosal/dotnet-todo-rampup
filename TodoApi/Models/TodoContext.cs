using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TodoApi.Controllers;

namespace TodoApi.Models
{
    public class TodoContext : DbContext, ITodoContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        protected virtual DbSet<TodoItem> TodoItems { get; set; }
        
        public void EnsureCreated()
        {
            Database.EnsureCreated();
        }

        public long Count()
        {
            return TodoItems.Count();
        }

        public IEnumerable<TodoItem> GetAll()
        {
            return TodoItems.AsEnumerable();
        }

        public TodoItem GetById(long id)
        {
            return TodoItems.FirstOrDefault(t => t.Id == id);
        }

        public TodoItem Add(TodoItem item)
        {
            TodoItems.Add(item);
            SaveChanges();
            return item;
        }

        public void Update(TodoItem item)
        {
            TodoItems.Update(item);
            SaveChanges();
        }

        public void Remove(TodoItem item)
        {
            TodoItems.Remove(item);
            SaveChanges();
        }
    }
}