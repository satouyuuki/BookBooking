using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookBooking.Models
{
    public class LoginViewModel
    {
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "メールアドレス")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "パスワード")]
        public string Password { get; set; }
    }
}