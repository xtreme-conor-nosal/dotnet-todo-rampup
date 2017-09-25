using System;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public bool IsComplete { set; get; }
    }
}