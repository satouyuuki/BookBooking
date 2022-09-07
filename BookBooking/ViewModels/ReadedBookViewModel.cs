using System;
using Microsoft.AspNetCore.Mvc;

namespace BookBooking.ViewModels
{
    public class ReadedBookViewModel
    {
        [HiddenInput]
        public int BookHistoryId { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public DateTime ReservedDate { get; set; }
        [HiddenInput]
        public DateTime ScheduledReturnDate { get; set; }
        [HiddenInput]
        public DateTime ReturnDate { get; set; }

        public bool canBorrow => ScheduledReturnDate == DateTime.MinValue && ReturnDate == DateTime.MinValue;
        public bool canReturn => ScheduledReturnDate != DateTime.MinValue && ReturnDate == DateTime.MinValue;
    }
}

