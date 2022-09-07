using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBooking.Models
{
    public class BookHistory
    {
        [Required]
        public int BookHistoryId { get; set; }

        [ForeignKey("UsersTable")]
        public int UserId { get; set; }
        public User UsersTable { get; set; }
        
        [ForeignKey("BooksTable")]
        public int BookId { get; set; }
        public Book BooksTable { get; set; }

        [Required]
        public DateTime ReservedDate { get; set; }
        [Required]
        public DateTime ScheduledReturnDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}

