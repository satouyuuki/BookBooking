using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookBooking.Areas.Api.Controllers
{
    [Area("Api")]
    public class BooksController : Controller
    {
        // TODO: API用のPATHを作りたかったらこのパスを使う
        // Path: /Api/Books/Index | /Api/Books | /Api
        [HttpGet]
        public JsonResult Index()
        {
            return new JsonResult("test api");
        }
    }
}

