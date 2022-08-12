using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBooking.Models
{
    public class BookHistoryViewModel
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }

        public BookUsage Usage { get; set; }

        public string GetImageUrl()
        {
            var baseDir = "Uploads";
            if (string.IsNullOrEmpty(ImageUrl))
                return $"{baseDir}/noImage.png";
            return $"{baseDir}/{ImageUrl}";
        }
    }
}

