using System;
using System.ComponentModel.DataAnnotations;

namespace BookBooking.Models
{
    public class LendingViewModel
    {
        [Display(Name = "バーコード")]
        public int BarcodeId { get; set; }
    }
}

