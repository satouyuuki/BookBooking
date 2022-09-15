using System;

namespace BookBooking.Models
{
    public class BookHistoryRepository : IBookHistoryRepository
    {
        private readonly BookContext context;

        public BookHistoryRepository(BookContext context)
        {
            this.context = context;
        }

        public BookHistory Add(BookHistory bookHistory)
        {
            context.BookHistory.Add(bookHistory);
            context.SaveChanges();
            return bookHistory;
        }

        public IEnumerable<BookHistory> GetAllBookHistory()
        {
            return context.BookHistory;
        }

        public BookHistory GetBookHistory(int id)
        {
            return context.BookHistory.Find(id);
        }

        public BookHistory Update(BookHistory bookHistoryChanges)
        {
            var bookHistory = context.BookHistory.Attach(bookHistoryChanges);
            bookHistory.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return bookHistoryChanges;
        }
    }
}

