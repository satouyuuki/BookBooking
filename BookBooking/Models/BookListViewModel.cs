using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;

namespace BookBooking.Models
{

    public class BookListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public BookBooking.Models.BookStatus.BookStatus Status { get; set; }

        public string GetImageUrl()
        {
            var baseDir = "Uploads";
            if (string.IsNullOrEmpty(ImageUrl))
                return $"{baseDir}/noImage.png";
            return $"{baseDir}/{ImageUrl}";
        }
    }
}

