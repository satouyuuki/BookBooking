using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace BookBooking.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        //private int _id;

        //public User() { }

        //public User(string name, string email, string password)
        //{
        //    Name = name;
        //    Email = email;
        //    Password = password;
        //}

        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Email { get; set; }
        [Required]
        [MaxLength(255)]
        public string? Password { get; set; }
    }
}

