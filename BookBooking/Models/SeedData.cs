using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BookBooking.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookContext(
                serviceProvider.GetRequiredService<
                DbContextOptions<BookContext>>()))
            {
                if (context.Books.Any())
                {
                    return; // DB has been seeded
                }

                context.Books.Add(
                    new Book
                    {
                        Title = "test book",
                        Description = "test book desc",
                        ImageUrl = "noImage.png"
                    });
                context.SaveChanges();

            }
        }
    }
}

