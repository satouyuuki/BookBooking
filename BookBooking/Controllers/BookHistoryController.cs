using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookBooking.Models;
using BookBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookBooking.Controllers
{
    public class BookHistoryController : ControllerBase
    {
        private readonly BookContext _context;
        private readonly ILogger<BookHistoryController> _logger;

        public BookHistoryController(
            ILogger<BookHistoryController> logger,
            BookContext context
            )
        {
            _logger = logger;
            _context = context;
        }

        // GET: /<controller>/
        public ActionResult<IEnumerable<BookHistoryViewModel>> Index()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(arg => arg.Type.Contains("primarysid")).Value);

            var viewModel = _context.BookHistory
                .Where(x => x.UserId == userId)
                .Join(
                _context.Books,
                bh => bh.BookId,
                b => b.Id,
                (bh, b) => new BookHistoryViewModel
                {
                    BookId = bh.BookId,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    Usage = new BookUsage(bh),
                }
                ).ToList();

            return View(viewModel);
        }

        //private BookUsage GetBookUsage(BookHistory bookHistory)
        //{
        //    //if (bookHistory == null) throw new ArgumentNullException();
        //    if (bookHistory.ScheduledReturnDate == DateTime.MinValue &&
        //        bookHistory.ReturnDate == DateTime.MinValue) return BookUsage.Reservation;
        //    else if (bookHistory.ScheduledReturnDate == DateTime.MinValue) return BookUsage.Cancelled;
        //    else if (bookHistory.ReturnDate == DateTime.MinValue) return BookUsage.Borrowed;
        //    return BookUsage.Returned;
        //}
    }
}

