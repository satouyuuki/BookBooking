using System;
using Microsoft.EntityFrameworkCore;

namespace BookBooking.Models
{
    public class BookContext: DbContext
    {
        public BookContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; } = null!;
    }
}

