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

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookHistory> BookHistory { get; set; }
    }
}

