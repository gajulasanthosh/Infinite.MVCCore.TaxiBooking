using Infinite.MVCCore.TaxiBooking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infinite.MVCCore.TaxiBooking.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IConfiguration _configuration;
        public string login2 = null;
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

                        //var check = User.FindFirstValue(ClaimTypes.Name);
                        
                        string role = await ExtractRole();
                        if (role == "Customer")
                        {
                            
                            return RedirectToAction("Create", "Bookings");
                        }
                        else if (role == "Admin")
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (role=="Employee")
                        {
                            return RedirectToAction("Index", "Employees");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
                    ModelState.AddModelError("", "Invalid LoginID or Password");
                }
            }
            return View(login);
        }

        [NonAction]
        public async Task<string> ExtractRole()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                //  var result = await client.GetAsync("Accounts/GetName");                
                var roleResult = await client.GetAsync("Accounts/GetRole");
                if (roleResult.IsSuccessStatusCode)
                {
                    // var name = await result.Content.ReadAsAsync<string>();
                    // ViewBag.Name = name;                    
                    var role = await roleResult.Content.ReadAsAsync<string>();
                    // ViewBag.Role = role; 
                    return role;
                }
                return null;
            }
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

                        //if (model.SelectedValue=="Customer")
                        //{
                        //    return RedirectToAction("Create", "Customers");
                        //}
                        //else if (model.SelectedValue=="Employee")
                        //{
                        //    return RedirectToAction("Create", "Employees");
                        //}
                        

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


