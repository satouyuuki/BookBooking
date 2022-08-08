using System;
using System.Security.Claims;
using BookBooking.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace BookBooking.Controllers
    {
        public class AccountController : ControllerBase
        {
            private readonly BookContext _context;
            private readonly ILogger<BooksController> _logger;

            public AccountController(
                ILogger<BooksController> logger,
                BookContext context
                )
            {
                _logger = logger;
                _context = context;
            }

            [HttpGet]
            public IActionResult Login(string returnUrl)
            {
                return View();
            }

            [HttpGet]
            public IActionResult Register()
            {
                return View();
            }

            [HttpPost]
            [ActionName(nameof(Login))]
            public async Task<IActionResult> PostLogin(LoginViewModel viewModel)
            {
                var users = _context.Users.ToList();
                var currentUser = users.FirstOrDefault(x => x.Email == viewModel.Email);
                if (currentUser == null)
                {
                    ModelState.AddModelError("Email", "メールアドレスが登録されていません。");
                    return View(viewModel);
                }
                var isVerfyPassword = new PasswordHasher<ClaimsPrincipal>().VerifyHashedPassword(User, currentUser.Password, viewModel.Password) == PasswordVerificationResult.Success;
                //else if (currentUser.Password != viewModel.Password)
                if (!isVerfyPassword)
                {
                    ModelState.AddModelError("Password", "パスワードが一致しません。");
                    return View(viewModel);
                }

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.PrimarySid, currentUser.Id.ToString()),
                    new Claim(ClaimTypes.Email, currentUser.Email),
                    new Claim(ClaimTypes.Role, currentUser.Role.ToString())
                };

                var identity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    principal);
                SetFlash(FlashMessageType.Success, "ログインに成功しました。");
                return RedirectToAction("Index", "Books");
            }

            [HttpPost]
            [ActionName(nameof(Register))]
            public IActionResult PostRegister(RegisterViewModel viewModel)
            {
                // validation
                if (!ModelState.IsValid)
                {
                    return View(viewModel);
                }
                var model = new User() {
                    Name = viewModel.Username,
                    Email = viewModel.Email,
                    Password = new PasswordHasher<ClaimsPrincipal>().HashPassword(User, viewModel.Password),
                    Role = viewModel.Role
                };

                _context.Add(model);
                try
                {
                    _context.SaveChanges();
                    SetFlash(FlashMessageType.Success, "登録に成功しました。");
                    return RedirectToAction(nameof(Login));
                }
                catch (DbUpdateException ex)
                when (ex.InnerException is MySqlException)
                {
                    if (ex.InnerException.Message.Contains("Duplicate"))
                    {
                        ModelState.AddModelError("Email", "メールアドレスが既に登録されています。");
                    }
                }
                return View(viewModel);
            }

            [Authorize]
            public async Task<IActionResult> Logout()
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction();
            }

            public class LoginRequest
            {
                public string username { get; set; }
                public string email { get; set; }
            }
        }
    }
