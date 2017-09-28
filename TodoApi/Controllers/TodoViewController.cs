using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("todo")]
    public class TodoViewController : Controller
    {
        private readonly TodoService _service;
        
        public TodoViewController(TodoService service)
        {
            _service = service;
        } 
        
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<TodoItem> items = _service.GetAll().ToList();
            ViewData["items"] = items;
            return View();
        }
        
        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var item = _service.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["item"] = item;
            return View();
        }
    }
}