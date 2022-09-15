using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookBooking.ViewModels
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

        [Display(Name = "自動ログイン機能")]
        public bool RememberMe { get; set; }
    }
}