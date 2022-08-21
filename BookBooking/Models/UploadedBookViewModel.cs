using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBooking.Models
{
    public class UploadedBookViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "タイトル")]
        public string Title { get; set; }
        [Display(Name = "説明")]
        public string Description { get; set; }
        [Display(Name = "サムネイル")]
        public IFormFile? File { get; set; }

        public string? ImageUrl { get; set; }

        public string GetImageUrl()
        {
            var baseDir = "Uploads";
            if (string.IsNullOrEmpty(ImageUrl))
                return $"{baseDir}/noImage.png";
            return $"{baseDir}/{ImageUrl}";
        }
    }
}

