using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GreenFarm.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GreenFarm.Controllers
{
    public class UserController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(User model)
        {
            using (Database db = new Database())
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
                if (user == null)
                {
                    db.Users.Add(new User
                    {
                        Password = model.Password,
                        Name = model.Name,
                        PhoneNumber = model.PhoneNumber
                    });
                    await db.SaveChangesAsync();

                    await Authenticate(model.PhoneNumber);

                    return RedirectToAction("Index", "Home");
                }
                else
                    ViewBag.Error = "Данный Номер телефона уже зарегистрирован!";
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(User model)
        {
            using (Database db = new Database())
            {
                var users = await db.Users.ToListAsync();
                User user = users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.PhoneNumber);

                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Error = "Некорректные номер телефона или пароль";
            }
            return View(model);
        }

        private async System.Threading.Tasks.Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
