using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookBooking.Models;
using BookBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;

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
    }
}

