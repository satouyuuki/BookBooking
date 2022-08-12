namespace BookBooking.Models
{
    public class BookUsage
    {
        public UsageType Value { get; }

        public DateTime ReservedDate { get; }

        public DateTime? ScheduledReturnDate { get; }

        public DateTime? ReturnedDate { get; }

        public DateTime? CancelledDate { get; }

        public BookUsage(BookHistory bookHistory)
        {
            if (bookHistory == null) throw new ArgumentException();

            ReservedDate = bookHistory.ReservedDate;

            if (bookHistory.ScheduledReturnDate == DateTime.MinValue &&
                bookHistory.ReturnDate == DateTime.MinValue)
            {
                Value = UsageType.Reservation;
            }
            else if (bookHistory.ScheduledReturnDate == DateTime.MinValue)
            {
                Value = UsageType.Cancelled;
                CancelledDate = bookHistory.ReturnDate;
            }
            else if (bookHistory.ReturnDate == DateTime.MinValue)
            {
                Value = UsageType.Borrowed;
                ScheduledReturnDate = bookHistory.ScheduledReturnDate;
            }
            else
            {
                Value = UsageType.Returned;
                ReturnedDate = bookHistory.ReturnDate;
            }
        }
        public enum UsageType
        {
            // 借りている
            Borrowed,
            // 予約中
            Reservation,
            // 返却済み
            Returned,
            // キャンセル済み
            Cancelled,
        }
    }
}

