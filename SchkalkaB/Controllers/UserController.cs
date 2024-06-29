using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchkalkaB.Data;
using SchkalkaB.Domain.Services;
using SchkalkaB.Models;
using SchkalkaB.ViewModels;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;

namespace SchkalkaB.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserInterface userService;
        SchkalkaDbContext context=new SchkalkaDbContext();

        public UserController(IUserInterface userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        private async Task SignIn(User user)
        {
            string role = user.Role switch
            {
                2 => "admin",
                1 => "user",
                _ => throw new ApplicationException("invalid user role")
            };
            List<Claim> claims = new List<Claim>
            {
                new Claim("id",user.UserId.ToString()),
                new Claim("role",role),
                new Claim("username",user.Login)
            };
            string auth = CookieAuthenticationDefaults.AuthenticationScheme;
            IIdentity identity=new ClaimsIdentity(claims,auth,"username","role");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            User user =await userService.GetUserAsync(login.UserName, login.Password);
            if (user is null)
            {
                ModelState.AddModelError("user_not_exist", "Not found");
                return View(login);
            }
            await SignIn(user);
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                User us = new User();
                us.UserId = user.UserId;
                us.Role = user.Role;
                us.Login = user.Login;
                us.Password = user.Password;
                await JsonSerializer.SerializeAsync<User>(fs, us);
            }
            List<Teacher> teachers=context.Teachers.ToList();
            List<Director> directors=context.Directors.ToList();
            Teacher? found = teachers.FirstOrDefault(u => u.Userl == user.UserId);
            Director? found1 = directors.FirstOrDefault(u => u.UserI == user.UserId);
            if (found != null)
            {
                return RedirectToAction("IndexTeacher", "Timetable");
            }
            else if (found1 != null)
            {
                return RedirectToAction("IndexDirector", "Timetable");
            }
            return RedirectToAction("Index", "Timetable");
        }

        public async Task<IActionResult> Logout()
        {
            using (StreamWriter writer = new StreamWriter(@"add.txt"))
            {
                writer.WriteLine(0);
            }
            using (StreamWriter writer = new StreamWriter(@"add1.txt"))
            {
                writer.WriteLine(0);
            }
            using (StreamWriter writer = new StreamWriter(@"add2.txt"))
            {
                writer.WriteLine(0);
            }
            await HttpContext.SignOutAsync();
            System.IO.File.WriteAllText(@"user.json", string.Empty);
            return RedirectToAction("Login", "User");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel registrationViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registrationViewModel);
            }
            if (await userService.IsUserExistsAsync(registrationViewModel.Username))
            {
                ModelState.AddModelError("user_exists", "Not found");
                return View(registrationViewModel);
            }

            try
            {
                await userService.RegistrationAsync(registrationViewModel.Username, registrationViewModel.Password);
                //return RedirectToAction("RegistrationSuccess", "User");
                return RedirectToAction("Adding", "Timetable");
            }
            catch
            {
                ModelState.AddModelError("reg_error", $"Не удалось зарегистрироваться, попробуйте попытку регистрации позже");
                return View(registrationViewModel);
            }
        }

        public IActionResult RegistrationSuccess()
        {
            return View();
        }
    }
}
