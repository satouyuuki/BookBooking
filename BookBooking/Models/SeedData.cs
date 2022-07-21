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
                        Title = "test1",
                        Description = "test1 desc",
                        ImageUrl = "20220712010812_buranko_girl_sad.png"
                    });
                context.SaveChanges();

            }
        }
    }
}

