using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly TodoService _service;
        
        public TodoController(TodoService service)
        {
            _service = service;
        } 
        
        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _service.GetAll().ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _service.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            
            _service.Create(item);
            
            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }
            
            var todo = _service.GetById(id);
            if (todo == null)
            {
                return NotFound();
            }
            
            _service.Update(item);

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(long id)
        {
            var todo = _service.GetById(id);
            if (todo == null)
            {
                return NotFound();
            }
            
            _service.DeleteById(id);

            return new NoContentResult();
        }
    }
}