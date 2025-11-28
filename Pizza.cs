using System;

namespace PizzaStore.Models
{
    public class Pizza
    {
        public int Id { get; set; } // Key property [cite: 60, 61]
        public string? Name { get; set; }
        public string? Description { get; set; } // [cite: 62]
    }
}