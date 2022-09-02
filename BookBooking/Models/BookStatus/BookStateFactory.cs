namespace BookBooking.Models.BookStatus
{
    internal static class BookStateFactory
    {
        private static AvailableForLendBook availableForLend => new AvailableForLendBook();
        private static BorrowedBook borrowed => new BorrowedBook();
        private static ReservationBook reservation => new ReservationBook();
        private static ReservedBook reserved => new ReservedBook();
        private static ReturningBook returning => new ReturningBook();

        internal static IBookStatus AvailableForLend => availableForLend;
        internal static IBookStatus Borrowed => borrowed;
        internal static IBookStatus Reservation => reservation;
        internal static IBookStatus Reserved => reserved;
        internal static IBookStatus Returning => returning;
    }
}

