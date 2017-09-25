using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    public class TodoService
    {
        private readonly TodoContext _context;
        
        public TodoService(TodoContext context)
        {
            _context = context;

            _context.Database.EnsureCreated();
            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        } 
        
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        public TodoItem GetById(long id)
        {
            return _context.TodoItems.FirstOrDefault(t => t.Id == id);
        }

        public TodoItem Create(TodoItem item)
        {
            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return item;
        }

        public void Update(TodoItem item)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == item.Id);
            
            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();
        }

        public void DeleteById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            
            _context.TodoItems.Remove(item);
            _context.SaveChanges();
        }
    }
}