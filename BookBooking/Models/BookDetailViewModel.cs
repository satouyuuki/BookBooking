using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BookBooking.Models
{
    [Bind]
    public class BookDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public BookStatus Status { get; set; }

        public string GetImageUrl()
        {
            var baseDir = "Uploads";
            if (string.IsNullOrEmpty(ImageUrl))
                return $"{baseDir}/noImage.png";
            return $"{baseDir}/{ImageUrl}";
        }

        public KeyValuePair<string, string> RouteName
        {
            get
            {
                switch (Status)
                {
                    case BookBooking.Models.BookStatus.AvailableForLend:
                        return new KeyValuePair<string, string>("Borrow", "借りる");
                    case BookBooking.Models.BookStatus.Borrowed:
                        return new KeyValuePair<string, string>("Return", "返す");
                    case BookBooking.Models.BookStatus.Reservation:
                        return new KeyValuePair<string, string>("Cancel", "キャンセルする");
                    case BookBooking.Models.BookStatus.Reserved:
                        return new KeyValuePair<string, string>("Reserve", "予約する");
                    default:
                        throw new Exception("book state error");
                }

            }
        }
    }
}

