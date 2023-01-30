using Infinite.MVCCore.TaxiBooking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Infinite.MVCCore.TaxiBooking.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IConfiguration _configuration;
        public AccountsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Accounts/Login", login);
                    if (result.IsSuccessStatusCode)
                    {
                        string token = await result.Content.ReadAsAsync<string>();
                        HttpContext.Session.SetString("token", token);
                        return RedirectToAction("Index", "Customers");

                    }
                    ModelState.AddModelError("", "Invalid LoginID or Password");
                }
            }
            return View(login);
        }

        [HttpPost]
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            RolesRegisterVM model = new RolesRegisterVM
            {
                Values = new List<SelectListItem>
                    {
                        new SelectListItem {Value = "Employee", Text="Employee"},
                        new SelectListItem{Value="Customer",Text="Customer"}
                    }
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RolesRegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear(); client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    RegisterViewModel user = new RegisterViewModel
                    {
                        LoginID = model.RegisterRoles.LoginID,
                        Password = model.RegisterRoles.Password,
                        ConfirmPassword = model.RegisterRoles.ConfirmPassword,
                        Role = model.SelectedValue
                    };

                    var result = await client.PostAsJsonAsync("Accounts/Register", user);
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Login");
                    }
                }
            }
            RegisterViewModel user1 = new RegisterViewModel
            {
                LoginID = model.RegisterRoles.LoginID,
                Password = model.RegisterRoles.Password,
                Role = model.SelectedValue
            };
            RolesRegisterVM model1 = new RolesRegisterVM
            {
                RegisterRoles = user1,
                Values = new List<SelectListItem>
                {
                    new SelectListItem { Value = "Customer", Text = "Customer" },
                    new SelectListItem { Value = "Employee", Text = "Employee" },
                }
            };
            return View(model1);
        }

    }


}


