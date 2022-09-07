using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookBooking.Models;

namespace BookBooking.ViewModels
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

