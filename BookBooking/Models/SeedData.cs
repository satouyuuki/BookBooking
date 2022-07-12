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

                context.Books.AddRange(
                    new Book
                    {
                        Id = 1,
                        Title = "test1",
                        Description = "test1 desc",
                        ImageUrl = "20220712010812_buranko_girl_sad.png"
                    },
                    new Book
                    {
                        Id = 2,
                        Title = "test2",
                        Description = "test2 desc",
                        ImageUrl = "20220712010812_buranko_girl_sad.png"
                    },
                    new Book
                    {
                        Id = 3,
                        Title = "test3",
                        Description = "test3 desc",
                        ImageUrl = "20220712010812_buranko_girl_sad.png"
                    },
                    new Book
                    {
                        Id = 4,
                        Title = "test4",
                        Description = "test4 desc",
                        ImageUrl = "20220712010812_buranko_girl_sad.png"
                    }
                    );
                context.SaveChanges();

            }
        }
    }
}

