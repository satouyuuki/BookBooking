using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace BookBooking.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
    }
}

