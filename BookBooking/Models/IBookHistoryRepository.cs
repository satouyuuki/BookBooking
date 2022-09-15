using System;
namespace BookBooking.Models
{
    public interface IBookHistoryRepository
    {
        BookHistory GetBookHistory(int id);
        IEnumerable<BookHistory> GetAllBookHistory();
        BookHistory Add(BookHistory bookHistory);
        BookHistory Update(BookHistory bookHistoryChanges);
    }
}

