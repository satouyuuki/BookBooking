﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BookBooking.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "名前")]
        public string Username { get; set; } = String.Empty;

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        //[Remote(action: "VerifyEmail", controller: "Account")]
        [Display(Name = "メールアドレス")]
        public string Email { get; set; } = String.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "パスワード")]
        public string Password { get; set; } = String.Empty;
    }
}