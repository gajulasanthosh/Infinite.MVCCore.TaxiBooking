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
    public class BookingsController : Controller
    {
        private readonly IConfiguration _configuration;

        public BookingsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            List<BookingViewModel> bookings = new();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Bookings/GetAllBookings");
                if (result.IsSuccessStatusCode)
                {
                    bookings = await result.Content.ReadAsAsync<List<BookingViewModel>>();
                }
            }
            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            BookingViewModel booking = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync($"Bookings/GetBookingById/{id}");
                if (result.IsSuccessStatusCode)
                {
                    booking = await result.Content.ReadAsAsync<BookingViewModel>();
                }
            }
            return View(booking);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            BookingViewModel viewModel = new BookingViewModel
            {
                TaxiModels = await this.GetTaxiModels(),
                TaxiTypes = await this.GetTaxiTypes()
            };
            return View(viewModel);
            

        }
        [HttpPost]
        public async Task<IActionResult> Create(BookingViewModel booking)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiUrl:api"]);
                    
                    var result = await client.PostAsJsonAsync("Bookings/CreateBooking", booking);
                    if (result.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            BookingViewModel viewModel = new BookingViewModel
            {
                TaxiModels = await this.GetTaxiModels()
            };
            return View(viewModel);
            //return View(booking);
        }
      [NonAction]
        public async Task<List<TaxiModelViewModel>> GetTaxiModels()
        {
            List<TaxiModelViewModel> models = new();
            using(var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Bookings/GetTaxiModels");
                if (result.IsSuccessStatusCode)
                {
                    models = await result.Content.ReadAsAsync<List<TaxiModelViewModel>>();
                }

            }

            return models;
        }
        [NonAction]
        public async Task<List<TaxiTypeViewModel>> GetTaxiTypes()
        {
            List<TaxiTypeViewModel> types = new();
            using(var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]);
                var result = await client.GetAsync("Bookings/GetTaxiTypes");
                if (result.IsSuccessStatusCode)
                {
                    types = await result.Content.ReadAsAsync<List<TaxiTypeViewModel>>();
                }
            }
            return types;
        }


        public async Task<IActionResult> ApproveBooking(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]); var res = await client.PutAsync($"Employees/ApproveBooking/{id}", null);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Bookings");
                }
                return BadRequest();
            }
        }

        [HttpPost("Bookings/RejectBooking/{BookingId}")]
        public async Task<IActionResult> RejectBooking(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new System.Uri(_configuration["ApiUrl:api"]); var res = await client.PutAsync($"Employees/RejectBookings/{id}", null);
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Bookings");
                }
                return BadRequest();
            }
        }
    }
}
    
