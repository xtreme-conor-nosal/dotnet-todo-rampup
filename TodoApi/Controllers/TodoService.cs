using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    public class TodoService
    {
        private readonly ITodoContext _context;
        private readonly ILogger<TodoService> _logger;
        
        public TodoService(ITodoContext context, ILogger<TodoService> logger)
        {
            _context = context;
            _logger = logger;

            _context.EnsureCreated();
            if (_context.Count() == 0)
            {
                _logger.LogInformation("Adding default item");
                _context.Add(new TodoItem { Name = "Item1" });
            }
        } 
        
        public IEnumerable<TodoItem> GetAll()
        {
            _logger.LogInformation("GetAll");
            return _context.GetAll();
        }

        public TodoItem GetById(long id)
        {
            _logger.LogInformation($"GetById {id}");
            return _context.GetById(id);
        }

        public TodoItem Create(TodoItem item)
        {
            _logger.LogInformation($"Create {item}");
            _context.Add(item);
            return item;
        }

        public void Update(TodoItem item)
        {
            _logger.LogInformation($"Update {item}");
            var todo = _context.GetById(item.Id);
            
            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.Update(todo);
        }

        public void DeleteById(long id)
        {
            _logger.LogInformation($"DeleteById {id}");
            var item = _context.GetById(id);
            
            _context.Remove(item);
        }
    }
}