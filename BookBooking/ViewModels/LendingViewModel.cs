using System;
using System.ComponentModel.DataAnnotations;

namespace BookBooking.ViewModels
{
    public class LendingViewModel
    {
        [Display(Name = "バーコード")]
        public int BarcodeId { get; set; }
    }
}

