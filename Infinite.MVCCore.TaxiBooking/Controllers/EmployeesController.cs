using Infinite.MVCCore.TaxiBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infinite.MVCCore.TaxiBooking.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IConfiguration _configuration;

        public EmployeesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<EmployeeViewModel> employees = new();
            using(var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Employees/GetAllEmployees");
                if (result.IsSuccessStatusCode)
                {
                    employees = await result.Content.ReadAsAsync<List<EmployeeViewModel>>();
                }
            }
            return View(employees);
        }

        public async Task<IActionResult> Details(int id)
        {
            EmployeeViewModel employee = null;
            using(var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Employees/GetEmployeeById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    employee = await result.Content.ReadAsAsync<EmployeeViewModel>();
                }
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("Employees/CreateEmployee", employee);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(employee);
        }
        //[HttpGet]
        //public async Task<IActionResult>Edit(int id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        EmployeeViewModel movie = null;
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
        //            var result = await client.GetAsync($"Movies/GetMovieById/{id}");
        //            if (result.IsSuccessStatusCode)
        //            {
        //                movie = await result.Content.ReadAsAsync<MovieViewModel>();
        //                movie.Genres = await this.GetGenres();
        //                return View(movie);
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("", "Movie doesn't exists");
        //            }
        //        }
        //    }
        //    return View();
        //}
    }
}
