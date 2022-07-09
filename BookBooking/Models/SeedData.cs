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
                        ImageUrl = "https://blogger.googleusercontent.com/img/a/AVvXsEiTjjaWkCHeCslxeKujaQotjuR_HvhOHen7Ql7rQV8nmQl4ivLRF_CSabMY-JLJHNL-xHMe3VDyA9F-iv1RtQ2jFTk1Vnjck-veKLUZPdYqCcjzHkc8SGKb1fyZ_x8W66aTrSXQ78UN3CnyBakwwmmH_hmPKeEsTzq8J3e2zzUMylmDjCZ0LClPByBRRw=s689"
                    },
                    new Book
                    {
                        Id = 2,
                        Title = "test2",
                        Description = "test2 desc",
                        ImageUrl = "https://blogger.googleusercontent.com/img/a/AVvXsEiTjjaWkCHeCslxeKujaQotjuR_HvhOHen7Ql7rQV8nmQl4ivLRF_CSabMY-JLJHNL-xHMe3VDyA9F-iv1RtQ2jFTk1Vnjck-veKLUZPdYqCcjzHkc8SGKb1fyZ_x8W66aTrSXQ78UN3CnyBakwwmmH_hmPKeEsTzq8J3e2zzUMylmDjCZ0LClPByBRRw=s689"
                    },
                    new Book
                    {
                        Id = 3,
                        Title = "test3",
                        Description = "test3 desc",
                        ImageUrl = "https://blogger.googleusercontent.com/img/a/AVvXsEiTjjaWkCHeCslxeKujaQotjuR_HvhOHen7Ql7rQV8nmQl4ivLRF_CSabMY-JLJHNL-xHMe3VDyA9F-iv1RtQ2jFTk1Vnjck-veKLUZPdYqCcjzHkc8SGKb1fyZ_x8W66aTrSXQ78UN3CnyBakwwmmH_hmPKeEsTzq8J3e2zzUMylmDjCZ0LClPByBRRw=s689"
                    },
                    new Book
                    {
                        Id = 4,
                        Title = "test4",
                        Description = "test4 desc",
                        ImageUrl = "https://blogger.googleusercontent.com/img/a/AVvXsEiTjjaWkCHeCslxeKujaQotjuR_HvhOHen7Ql7rQV8nmQl4ivLRF_CSabMY-JLJHNL-xHMe3VDyA9F-iv1RtQ2jFTk1Vnjck-veKLUZPdYqCcjzHkc8SGKb1fyZ_x8W66aTrSXQ78UN3CnyBakwwmmH_hmPKeEsTzq8J3e2zzUMylmDjCZ0LClPByBRRw=s689"
                    }
                    );
                context.SaveChanges();

            }
        }
    }
}

