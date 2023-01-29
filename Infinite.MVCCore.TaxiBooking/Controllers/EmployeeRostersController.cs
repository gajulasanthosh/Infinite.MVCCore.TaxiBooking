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
    public class EmployeeRostersController : Controller
    {
        private readonly IConfiguration _configuration;

        public EmployeeRostersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            List<EmployeeRosterViewModel> employeeRosters = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("EmployeeRosters/GetAllRosters");
                if (result.IsSuccessStatusCode)
                {
                    employeeRosters = await result.Content.ReadAsAsync<List<EmployeeRosterViewModel>>();
                }
            }

            return View(employeeRosters);
        }
        public async Task<IActionResult> Details(int id)
        {
            EmployeeRosterViewModel employeeRoster = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"EmployeeRosters/GetRosterById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    employeeRoster = await result.Content.ReadAsAsync<EmployeeRosterViewModel>();
                }
            }
            return View(employeeRoster);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeRosterViewModel employee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PostAsJsonAsync("EmployeeRosters/CreateRoster", employee);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(employee);
        }

        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                EmployeeRosterViewModel employee = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"EmployeeRosters/GetRosterById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        employee = await result.Content.ReadAsAsync<EmployeeRosterViewModel>();
                        return View(employee);
                    }
                    //else
                    //{
                    //    //ModelState.AddModelError("", "Customer doesn't exist");
                    //}

                }
            }
            return View();
        }

        [HttpPost]
        [Route("EmployeeRosters/Edit/{Id}")]
        public async Task<IActionResult> Edit(EmployeeRosterViewModel employee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"EmployeeRosters/UpdateRoster/{employee.Id}", employee);
                    if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return RedirectToAction("Index");
                    }
                    //else
                    //{
                    //    ModelState.AddModelError("", "Server Error, Please try later");
                    //}

                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            EmployeeRosterViewModel employee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"EmployeeRosters/GetRosterById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    employee = await result.Content.ReadAsAsync<EmployeeRosterViewModel>();
                    return View(employee);
                }
                //else
                //{
                //    ModelState.AddModelError("", "Server Error.Please try later");
                ////}
            }
            return View(employee);
        }

        [HttpPost]
        [Route("EmployeeRosters/Delete/{Id}")]
        public async Task<IActionResult> Delete(EmployeeRosterViewModel employee)
        {
            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"EmployeeRosters/DeleteRoster/{employee.Id}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                //else
                //{
                //    return RedirectToAction("Login", "Accounts");
                //}
            }
            return View(employee);
        }
    }
}
