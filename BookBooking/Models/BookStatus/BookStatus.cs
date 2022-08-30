using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Reflection;
using BookBooking.Models.BookStatus;

namespace BookBooking.Models.BookStatus
{
    public class BookStatus
    {
        public IBookStatus Item { get; }

        public BookStatus(int userId, List<BookHistory> notReturnedBookList)
        {
            if (notReturnedBookList == null || notReturnedBookList.Count() == 0)
            {
                Item = BookStateFactory.AvailableForLend;
                return;
            }

            var isReservation = notReturnedBookList.Any(x =>
                x.UserId == userId &&
                x.ScheduledReturnDate == DateTime.MinValue);
            var isBorrowed = notReturnedBookList.Any(x =>
                x.UserId == userId &&
                x.ScheduledReturnDate != DateTime.MinValue);
            var isReserved = notReturnedBookList.Any(x =>
                x.UserId != userId);

            // 自分が借りている
            if (isBorrowed)
                Item = BookStateFactory.Borrowed;
            // 自分が予約中である
            else if (isReservation)
                Item = BookStateFactory.Reservation;
            // 他人が予約中である
            else if (isReserved)
                Item = BookStateFactory.Reserved;
            // それ以外: 予約可能である
            else
                Item = BookStateFactory.AvailableForLend;
            // それ以外
            // throw new ArgumentOutOfRangeException();
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
        public string NextActionName => "Cancel";

        public string DisplayNextActionName => "キャンセルする";

        public BookStatusEnum Status => BookStatusEnum.Reservation;
    }

    internal class ReservedBook : IBookStatus
    {
        public string NextActionName => "Reserve";

        public string DisplayNextActionName => "予約する";

        public BookStatusEnum Status => BookStatusEnum.Reserved;
    }
}

