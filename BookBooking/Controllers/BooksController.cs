using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookBooking.Migrations;
using BookBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
//using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookBooking.Controllers
{
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly BookContext _context;
        private readonly ILogger<BooksController> _logger;
        private IHostingEnvironment mxHostingEnvironment { get; set; }

        public BooksController(
            ILogger<BooksController> logger,
            BookContext context,
            IHostingEnvironment hostingEnvironment
            )
        {
            _logger = logger;
            _context = context;
            mxHostingEnvironment = hostingEnvironment;
        }

        public ActionResult<IEnumerable<BookListViewModel>> Index()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(arg => arg.Type.Contains("primarysid")).Value);
            var books = _context.Books.ToList();
            var bookHistory = _context.BookHistory.ToList();

            var viewModel = books.Select(x => {
                return new BookListViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    Status = GetStatus(x.Id, userId, bookHistory)
                };
            });
            return View(viewModel);
        }

        private BookStatus GetStatus(int bookId, int userId, List<BookHistory> bookHistories)
        {
            if (bookHistories == null || bookHistories.Count == 0) return BookStatus.AvailableForLend;
            var notReturnedBookList = bookHistories.Where(x => DateTime.MinValue == x.ReturnDate);
            var isReservation = notReturnedBookList.Any(x =>
                x.BookId == bookId &&
                x.UserId == userId &&
                x.ScheduledReturnDate == DateTime.MinValue);
            var isBorrowed = notReturnedBookList.Any(x =>
                x.BookId == bookId &&
                x.UserId == userId &&
                x.ScheduledReturnDate != DateTime.MinValue);
            var isReserved = notReturnedBookList.Any(x => x.BookId == bookId && x.UserId != userId);

            // 全ての本が返却ずみ
            if (notReturnedBookList == null || notReturnedBookList.Count() == 0) return BookStatus.AvailableForLend;
            // 自分が借りている
            if (isBorrowed) return BookStatus.Borrowed;
            // 自分が予約中である
            if (isReservation) return BookStatus.Reservation;
            // 他人が予約中である
            if (isReserved) return BookStatus.Reserved;
            // それ以外: 予約可能である
            return BookStatus.AvailableForLend;
            // TODO: 禁止である
            //return BookStatus.Restriction;
        }


        // GET api/values/5
        public async Task<ActionResult<Book>> Detail(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(arg => arg.Type.Contains("primarysid")).Value);
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            var bookHistories = _context.BookHistory.ToList();

            if (book == null)
            {
                return NotFound();
            }

            return View(new BookDetailViewModel
            {
                Id = book.Id,
                Description = book.Description,
                Title = book.Title,
                ImageUrl = book.ImageUrl,
                Status = GetStatus(book.Id, userId, bookHistories)
            });
        }

        [HttpPost("Books/Detail/{id:int}/Borrow", Name = "Borrow")]
        public async Task<ActionResult> Borrow(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(arg => arg.Type.Contains("primarysid")).Value);
            var bookHistory = new BookHistory
            {
                BookId = id,
                UserId = userId,
                ReservedDate = DateTime.Now,
                ScheduledReturnDate = DateTime.Now.AddDays(7)
            };
            _context.BookHistory.Add(bookHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", new { id = id });
        }

        [HttpPost("Books/Detail/{id:int}/Return", Name = "Return")]
        public async Task<ActionResult> Return(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(arg => arg.Type.Contains("primarysid")).Value);
            var bookHistory = _context.BookHistory.FirstOrDefault(x =>
                x.BookId == id &&
                x.UserId == userId &&
                x.ScheduledReturnDate != DateTime.MinValue);
            bookHistory.ReturnDate = DateTime.Now;

            _context.BookHistory.Update(bookHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", new { id = id });
        }

        [HttpPost("Books/Detail/{id:int}/Reserve", Name = "Reserve")]
        public async Task<ActionResult> Reserve(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(arg => arg.Type.Contains("primarysid")).Value);
            var bookHistory = new BookHistory
            {
                BookId = id,
                UserId = userId,
                ReservedDate = DateTime.Now
            };
            _context.BookHistory.Add(bookHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", new { id = id });
        }

        [HttpPost("Books/Detail/{id:int}/Cancel", Name = "Cancel")]
        public async Task<ActionResult> Cancel(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(arg => arg.Type.Contains("primarysid")).Value);
            var bookHistory = _context.BookHistory.FirstOrDefault(x =>
                x.BookId == id &&
                x.UserId == userId &&
                x.ScheduledReturnDate == DateTime.MinValue);
            bookHistory.ReturnDate = DateTime.Now;

            _context.BookHistory.Update(bookHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", new { id = id });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        // POST api/values
        public async Task<ActionResult<Book>> Create(UploadedBookViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var model = new Book
            {
                Title = viewModel.Title,
                Description = viewModel.Description
            };

            if (viewModel.File != null && viewModel.File.Length > 0)
            {
                //upload files to wwwroot
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss_") +
                    Path.GetFileName(viewModel.File.FileName);
                //var filePath = Path.Combine(mxHostingEnvironment.ContentRootPath, "Uploads", fileName);
                var filePath = Path.Combine(mxHostingEnvironment.WebRootPath, "Uploads", fileName);

                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.File.CopyToAsync(fileSteam);
                }
                //your logic to save filePath to database
                model.ImageUrl = fileName;
            }

            _context.Books.Add(model);
            await _context.SaveChangesAsync();
            SetFlash(FlashMessageType.Success, "本の登録に成功しました。");
            return RedirectToAction(nameof(Index));
        }

        // get: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UploadedBookViewModel>> Edit(int id)
        {
            var model = await _context.Books.FindAsync(id);

            var viewModel = new UploadedBookViewModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl
            };
            return View(viewModel);
        }

        // PUT api/values/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, UploadedBookViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }
            //_context.Entry(book).State = EntityState.Modified;
            if (!ModelState.IsValid) return View(new { id = id });

            var model = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (string.IsNullOrWhiteSpace(viewModel.Title)) model.Title = viewModel.Title;
            if (string.IsNullOrWhiteSpace(viewModel.Description)) model.Description = viewModel.Description;


            if (viewModel.File != null && viewModel.File.Length > 0)
            {
                //upload files to wwwroot
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss_") +
                    Path.GetFileName(viewModel.File.FileName);
                //var filePath = Path.Combine(mxHostingEnvironment.ContentRootPath, "Uploads", fileName);
                var filePath = Path.Combine(mxHostingEnvironment.WebRootPath, "Uploads", fileName);

                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.File.CopyToAsync(fileSteam);
                }
                //your logic to save filePath to database
                model.ImageUrl = fileName;
            }

            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            SetFlash(FlashMessageType.Success, "本の更新に成功しました。");
            return RedirectToAction(nameof(Index));
        }

        // DELETE api/values/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            SetFlash(FlashMessageType.Success, "本の削除に成功しました。");
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(book => book.Id == id);
        }
    }
}