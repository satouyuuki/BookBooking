using System;
namespace BookBooking.Models.BookStatus
{
    public interface IBookStatus
    {
        string NextActionName { get; }
        string DisplayNextActionName { get; }
        BookStatusEnum Status { get; }
    }
}

