using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

namespace BookBooking.ViewModels
{
    public class UserInfoViewModel
    {
        [Required]
        [HiddenInput]
        public int Id { get; set; }

        [Required]
        [Display(Name = "名前")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "メールアドレス")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "パスワード")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "新しいパスワード(変更したくない場合は空欄のままにしてください)")]
        public string? NewPassword { get; set; }
    }
}

