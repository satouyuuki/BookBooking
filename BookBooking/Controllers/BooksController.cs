using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using BookBooking.Migrations;
using BookBooking.Models;
using BookBooking.Models.BookStatus;
using BookBooking.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlConnector;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BookBooking.Controllers
{
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;
        private readonly IBookRepository _bookRepository;
        private readonly IBookHistoryRepository _bookHistoryRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public BooksController(
            ILogger<BooksController> logger,
            IBookRepository bookRepository,
            IBookHistoryRepository bookHistoryRepository,
            IHostingEnvironment hostingEnvironment
            )
        {
            _logger = logger;
            _bookRepository = bookRepository;
            _bookHistoryRepository = bookHistoryRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public ActionResult<IEnumerable<BookListViewModel>> Index()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(arg => arg.Type.Contains("primarysid")).Value);
            var books = _bookRepository.GetAllBook();
            var notReturnedbookHistories = _bookHistoryRepository.GetAllBookHistory()
                 .Where(bh =>
                    bh.ReturnDate == DateTime.MinValue)
                .ToList();

            var viewModel = books.Select(x => {
                return new BookListViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    ImageUrl = x.ImageUrl,
                    Status = new BookStatus(userId, notReturnedbookHistories.Where(bh => bh.BookId == x.Id).ToList())
                };
            });
            return View(viewModel);
        }

        public ActionResult<BookDetailViewModel> Detail(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(arg => arg.Type.Contains("primarysid")).Value);
            var book = _bookRepository.GetBook(id);
            if (book == null) return NotFound();
            // userIdを絞らないことで何件予約が入ってるかが分かる
            // TODO: 予定日をすぎてるものに関してはフィルタをかけると良いかも
            var notReturnedbookHistories = _bookHistoryRepository.GetAllBookHistory()
                .Where(bh =>
                bh.BookId == book.Id &&
                bh.ReturnDate == DateTime.MinValue)
                .ToList();

            return View(new BookDetailViewModel
            {
                Id = book.Id,
                Description = book.Description,
                Title = book.Title,
                ImageUrl = book.ImageUrl,
                Status = new BookStatus(userId, notReturnedbookHistories)
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
            };
            _bookHistoryRepository.Add(bookHistory);
            return RedirectToAction("Detail", new { id = id });
        }

        [HttpPost("Books/Detail/{id:int}/Return", Name = "Return")]
        public async Task<ActionResult> Return(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(arg => arg.Type.Contains("primarysid")).Value);
            var bookHistory = _bookHistoryRepository.GetAllBookHistory()
                .FirstOrDefault(x =>
                    x.BookId == id &&
                    x.UserId == userId &&
                    x.ScheduledReturnDate != DateTime.MinValue &&
                    x.ReturnDate == DateTime.MinValue);
            bookHistory.IsCompleted = false;

            _bookHistoryRepository.Update(bookHistory);
            SetFlash(FlashMessageType.Success, "処理が完了しました。管理者にコードを提示してください");
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
            _bookHistoryRepository.Add(bookHistory);
            return RedirectToAction("Detail", new { id = id });
        }

        [HttpGet("Books/Detail/{bookHistoryId:int}/ShowBarCode", Name = "ShowBarCode")]
        public IActionResult ShowBarCode(int bookHistoryId)
        {
            return PartialView("_BarCode", bookHistoryId.ToString());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Lending()
        {
            return View(new LendingViewModel());
        }

        [HttpGet]
        public IActionResult SearchLending(string id)
        {
            int bookHistoryId;
            int.TryParse(id, out bookHistoryId);
            var bookHistory = _bookHistoryRepository.GetBookHistory(bookHistoryId);
            if(bookHistory == null)
            {
                return View();
            }

            var book = _bookRepository.GetBook(bookHistory.BookId);
            var viewModel = new ReadedBookViewModel
            {
                BookHistoryId = bookHistory.BookHistoryId,
                Title = book.Title,
                UserId = bookHistory.UserId,
                ReservedDate = bookHistory.ReservedDate,
                ScheduledReturnDate = bookHistory.ScheduledReturnDate,
                ReturnDate = bookHistory.ReturnDate,
            };
            return PartialView("_ReadedBook", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Lending(ReadedBookViewModel viewModel)
        {
            var bookHistory = _bookHistoryRepository.GetBookHistory(viewModel.BookHistoryId);
            if (viewModel.canBorrow)
            {
                bookHistory.ScheduledReturnDate = DateTime.Now.AddDays(7);
                bookHistory.IsCompleted = true;
            }
            if (viewModel.canReturn)
            {
                bookHistory.ReturnDate = DateTime.Now;
                bookHistory.IsCompleted = true;
            }
            _bookHistoryRepository.Update(bookHistory);
            SetFlash(FlashMessageType.Success, "処理が完了しました");
            return View();
        }

        [HttpPost("Books/Detail/{id:int}/Cancel", Name = "Cancel")]
        public async Task<ActionResult> Cancel(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(arg => arg.Type.Contains("primarysid")).Value);
            var bookHistory = _bookHistoryRepository.GetAllBookHistory()
                .FirstOrDefault(x =>
                    x.BookId == id &&
                    x.UserId == userId &&
                    x.ScheduledReturnDate == DateTime.MinValue);
            bookHistory.ReturnDate = DateTime.Now;

            _bookHistoryRepository.Update(bookHistory);
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
        public async Task<ActionResult<Book>> Create(UploadedBookViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var model = new Book
            {
                Title = viewModel.Title,
                Description = viewModel.Description
            };

            if (viewModel.File != null)
            {
                //upload files to wwwroot
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss_") +
                    Path.GetFileName(viewModel.File.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", fileName);

                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.File.CopyToAsync(fileSteam);
                }
                model.ImageUrl = fileName;
            }

            _bookRepository.Add(model);
            SetFlash(FlashMessageType.Success, "本の登録に成功しました。");
            return RedirectToAction(nameof(Index));
        }

        // get: Books/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UploadedBookViewModel>> Edit(int id)
        {
            var model = _bookRepository.GetBook(id);

            var viewModel = new UploadedBookViewModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(UploadedBookViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var model = _bookRepository.GetBook(viewModel.Id);
            model.Title = viewModel.Title;
            model.Description = viewModel.Description;

            if (viewModel.File != null)
            {
                if (viewModel.ImageUrl != null)
                {
                    var imgFilePath = Path.Combine(_hostingEnvironment.WebRootPath,
                            "Uploads", model.ImageUrl);
                    System.IO.File.Delete(imgFilePath);
                }
                //upload files to wwwroot
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmss_") +
                    Path.GetFileName(viewModel.File.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads", fileName);
                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    await viewModel.File.CopyToAsync(fileSteam);
                }
                model.ImageUrl = fileName;
            }

            try
            {
                _bookRepository.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(viewModel.Id))
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletedBook = _bookRepository.Delete(id);
                if (deletedBook != null && deletedBook.ImageUrl != null)
                {
                    var imgFilePath = Path.Combine(_hostingEnvironment.WebRootPath,
                            "Uploads", deletedBook.ImageUrl);
                    System.IO.File.Delete(imgFilePath);
                    SetFlash(FlashMessageType.Success, "本の削除に成功しました。");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (MySqlException ex)
            {
                _logger.LogError($"本の削除に失敗しました。id: {id}", ex);
            }
            SetFlash(FlashMessageType.Error, "本の削除に失敗しました。");
            return RedirectToAction("Detail", new { id = id });
        }

        private bool BookExists(int id)
        {
            return _bookRepository.GetAllBook()
                .Any(book => book.Id == id);
        }
    }
}