using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookBooking.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly BookContext _context;
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILogger<BooksController> logger, BookContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Book>>> Index()
        {
            return View(await _context.Books.ToListAsync());
        }


        // GET api/values/5
        public async Task<ActionResult<Book>> Detail(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST api/values
        public async Task<ActionResult<Book>> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // get: Books/Edit/5
        public async Task<ActionResult<Book>> Edit(int id)
        {
            var book = await _context.Books.FindAsync(id);
            return View(book);
        }

        // PUT api/values/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }
            //_context.Entry(book).State = EntityState.Modified;
            if (ModelState.IsValid)
            {
                try
                {
                    //await _context.SaveChangesAsync();
                    _context.Update(book);
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
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // DELETE api/values/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(book => book.Id == id);
        }
    }
}