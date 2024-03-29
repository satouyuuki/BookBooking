﻿using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Reflection;
using BookBooking.Models.BookStatus;

namespace BookBooking.Models.BookStatus
{
    public class BookStatus
    {
        public int BookHistoryId { get; }
        public IBookStatus Item { get; }

        public BookStatus(int userId, List<BookHistory> notReturnedBookList)
        {
            if (notReturnedBookList == null || notReturnedBookList.Count() == 0)
            {
                Item = BookStateFactory.AvailableForLend;
                return;
            }

            var firstReservation = notReturnedBookList
                .OrderByDescending(x => x.ReservedDate)
                .FirstOrDefault(x =>
                x.UserId == userId &&
                x.ScheduledReturnDate == DateTime.MinValue);

            var bollowing = notReturnedBookList
                .OrderByDescending(x => x.ReservedDate)
                .FirstOrDefault(x =>
                x.UserId == userId &&
                x.ScheduledReturnDate != DateTime.MinValue &&
                x.IsCompleted == true
                );

            var returning = notReturnedBookList
                .OrderByDescending(x => x.ReservedDate)
                .FirstOrDefault(x =>
                x.UserId == userId &&
                x.ScheduledReturnDate != DateTime.MinValue &&
                x.IsCompleted == false
                );

            // 自分が予約中である
            if (firstReservation != null)
            {
                Item = BookStateFactory.Reservation;
                BookHistoryId = firstReservation.BookHistoryId;
            }
            // 自分が借りている
            else if (bollowing != null)
            {
                Item = BookStateFactory.Borrowed;
                BookHistoryId = bollowing.BookHistoryId;
            }
            // 返却中
            else if (returning != null)
            {
                Item = BookStateFactory.Returning;
                BookHistoryId = returning.BookHistoryId;
            }
            // 他人が予約中である
            else
            {
                Item = BookStateFactory.Reserved;
            }
            // それ以外
            //throw new ArgumentOutOfRangeException();

            // 古い仕様
            //var isReservation = notReturnedBookList.Any(x =>
            //    x.UserId == userId &&
            //    x.ScheduledReturnDate == DateTime.MinValue);
            //var isBorrowed = notReturnedBookList.Any(x =>
            //    x.UserId == userId &&
            //    x.ScheduledReturnDate != DateTime.MinValue);
            //var isReserved = notReturnedBookList.Any(x =>
            //    x.UserId != userId);

            // 自分が借りている
            //if (isBorrowed)
            //    Item = BookStateFactory.Borrowed;
            // それ以外: 予約可能である
            //else
            //    Item = BookStateFactory.AvailableForLend;

            // TODO: 禁止である
            //return BookStatus.Restriction;
        }
    }
    public class AvailableForLendBook : IBookStatus
    {
        public string NextActionName => "Borrow";

        public string DisplayNextActionName => "借りる";

        public BookStatusEnum Status => BookStatusEnum.AvailableForLend;
    }

    internal class BorrowedBook : IBookStatus
    {
        public string NextActionName => "Return";

        public string DisplayNextActionName => "返す";

        public BookStatusEnum Status => BookStatusEnum.Borrowed;
    }

    internal class ReservationBook : IBookStatus
    {
        public string NextActionName => "ShowBarCode";

        public string DisplayNextActionName => "バーコードを提示する";

        public BookStatusEnum Status => BookStatusEnum.Reservation;
    }

    internal class ReturningBook : IBookStatus
    {
        public string NextActionName => "ShowBarCode";

        public string DisplayNextActionName => "バーコードを提示する";

        public BookStatusEnum Status => BookStatusEnum.Returning;
    }

    internal class ReservedBook : IBookStatus
    {
        public string NextActionName => "Reserve";

        public string DisplayNextActionName => "予約する";

        public BookStatusEnum Status => BookStatusEnum.Reserved;
    }
}

