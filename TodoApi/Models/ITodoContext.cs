using System.Collections.Generic;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    public interface ITodoContext
    {
        void EnsureCreated();
        long Count();
        IEnumerable<TodoItem> GetAll();
        TodoItem GetById(long id);
        TodoItem Add(TodoItem item);
        void Update(TodoItem item);
        void Remove(TodoItem item);
    }
}