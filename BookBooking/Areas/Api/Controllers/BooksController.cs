using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookBooking.Areas.Api.Controllers
{
    [Area("Api")]
    public class BooksController : Controller
    {
        // TODO: API用のPATHを作りたかったらこのパスを使う
        // /Api/Books/Index
        // /Api/Books
        // /Api
        [HttpGet]
        public JsonResult Index()
        {
            string[] weekDays = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            //return Json(weekDays);
            return new JsonResult(Ok(weekDays));
        }
    }
}

