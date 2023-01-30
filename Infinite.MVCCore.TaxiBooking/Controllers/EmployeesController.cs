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
        public async Task<IActionResult> Create()
        {
            EmployeeViewModel viewModel = new EmployeeViewModel
            {
                Designations = await this.GetDesignations()
            };
            return View(viewModel);
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
            EmployeeViewModel viewModel = new EmployeeViewModel
            {
                Designations = await this.GetDesignations()
            };
            return View(viewModel);
          
        }
        [HttpGet]

        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                EmployeeViewModel employee = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.GetAsync($"Employees/GetEmployeeById/{id}");
                    if (result.IsSuccessStatusCode)
                    {
                        employee = await result.Content.ReadAsAsync<EmployeeViewModel>();
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
        [Route("Employees/Edit/{EmployeeId}")]
        public async Task<IActionResult> Edit(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    var result = await client.PutAsJsonAsync($"Employees/UpdateEmployee/{employee.EmployeeId}", employee);
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
            EmployeeViewModel employee = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Employees/GetEmployeeById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    employee = await result.Content.ReadAsAsync<EmployeeViewModel>();
                    return View(employee);
                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");
                }
            }
            return View(employee);
        }

        [HttpPost]
        [Route("Employees/Delete/{EmployeeId}")]
        public async Task<IActionResult> Delete(EmployeeViewModel employee)
        {
            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token"));
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.DeleteAsync($"Employees/DeleteEmployee/{employee.EmployeeId}");
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Server Error.Please try later");
                }
            }
            return View();
        }
      [NonAction]
      public async Task<List<DesignationViewModel>> GetDesignations()
        {
            List<DesignationViewModel> designations = new();
            using(var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Employees/GetDesignations");
                if (result.IsSuccessStatusCode)
                {
                    designations = await result.Content.ReadAsAsync<List<DesignationViewModel>>();
                }

            }
            return designations;
        }


    }
}
