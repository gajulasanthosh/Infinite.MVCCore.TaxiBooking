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
    public class AdminController : Controller
    {
        public readonly IConfiguration _configuration;
        public IActionResult Index()
        {
            return View();
        }

        public AdminController( IConfiguration configuration)
        {
            _configuration= configuration;
        }

        public async Task<IActionResult> Customers()
        {

            List<CustomerViewModel> customers = new();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Customers/GetAllCustomers");
                if (result.IsSuccessStatusCode)
                {
                    customers = await result.Content.ReadAsAsync<List<CustomerViewModel>>();
                }
            }
            return View(customers);
           
        }
        public async Task<IActionResult> Employees()
        {
            List<EmployeeViewModel> employees = new();
            using (var client = new HttpClient())
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
        public async Task<IActionResult> EmployeeRoster()
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
    }
}
