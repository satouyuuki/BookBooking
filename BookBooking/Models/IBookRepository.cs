using System;
namespace BookBooking.Models
{
    public interface IBookRepository
    {
        Book GetBook(int id);
        IEnumerable<Book> GetAllBook();
        Book Add(Book book);
        Book Update(Book bookChanges);
        Book Delete(int id);
    }
}

