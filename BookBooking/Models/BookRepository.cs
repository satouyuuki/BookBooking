using System;

namespace BookBooking.Models
{
    public class BookRepository : IBookRepository
    {
        private readonly BookContext context;

        public BookRepository(BookContext context)
        {
            this.context = context;
        }

        public Book Add(Book book)
        {
            context.Books.Add(book);
            context.SaveChanges();
            return book;
        }

        public Book Delete(int id)
        {
            Book book = context.Books.Find(id);
            if(book != null)
            {
                context.Books.Remove(book);
                context.SaveChanges();
            }
            return book;
        }

        public IEnumerable<Book> GetAllBook()
        {
            return context.Books;
        }

        public Book GetBook(int id)
        {
            return context.Books.Find(id);
        }

        public Book Update(Book bookChanges)
        {
            var book = context.Books.Attach(bookChanges);
            book.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return bookChanges;
        }
    }
}

