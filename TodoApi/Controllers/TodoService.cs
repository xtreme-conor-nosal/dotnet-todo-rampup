using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    public class TodoService
    {
        private readonly ITodoContext _context;
        
        public TodoService(ITodoContext context)
        {
            _context = context;

            _context.EnsureCreated();
            if (_context.Count() == 0)
            {
                _context.Add(new TodoItem { Name = "Item1" });
            }
        } 
        
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.GetAll();
        }

        public TodoItem GetById(long id)
        {
            return _context.GetById(id);
        }

        public TodoItem Create(TodoItem item)
        {
            _context.Add(item);
            return item;
        }

        public void Update(TodoItem item)
        {
            var todo = _context.GetById(item.Id);
            
            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.Update(todo);
        }

        public void DeleteById(long id)
        {
            var item = _context.GetById(id);
            
            _context.Remove(item);
        }
    }
}