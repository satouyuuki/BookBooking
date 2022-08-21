using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using BookBooking.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookBooking.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserInfomationController : ControllerBase
    {
        private readonly BookContext _context;
        private readonly ILogger<UserInfomationController> _logger;

        public UserInfomationController(
            ILogger<UserInfomationController> logger,
            BookContext context
            )
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.Where(x => x.Role != UserRole.Admin).ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var selectedUser = _context.Users.FirstOrDefault(x => x.Id == id);
            return View(selectedUser);
        }

        [HttpPost]
        public IActionResult Edit(UserRole role, int id)
        {
            if (id == 0) return RedirectToAction(nameof(Index));
            var updatedUser = _context.Users.FirstOrDefault(x => x.Id == id);
            if (updatedUser == null) return RedirectToAction(nameof(Index));

            updatedUser.Role = role;
            _context.Update(updatedUser);
            _context.SaveChanges();

            SetFlash(FlashMessageType.Success, "更新に成功しました。");
            return View(updatedUser);
        }

        [HttpPost]
        public void Delete(int id)
        {
            var deleteduser = _context.Users.FirstOrDefault(x => x.Id == id);
            if (deleteduser == null) RedirectToAction(nameof(Index));
            _context.Remove(deleteduser);
            _context.SaveChanges();
            SetFlash(FlashMessageType.Success, "削除に成功しました。");
            RedirectToAction(nameof(Index));
        }
    }
}

