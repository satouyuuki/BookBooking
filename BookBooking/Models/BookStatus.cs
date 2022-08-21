﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BookBooking.Models
{
    public enum BookStatus
    {
        // 貸し出し可能
        [Display(Name = "貸し出し可能")]
        AvailableForLend,
        // 借りている
        [Display(Name = "借りている")]
        Borrowed,
        // 予約中
        [Display(Name = "予約中")]
        Reservation,
        // 予約されている
        [Display(Name = "予約されている")]
        Reserved,
        // 貸出禁止
        [Display(Name = "貸出禁止")]
        Restriction,
    }
}

