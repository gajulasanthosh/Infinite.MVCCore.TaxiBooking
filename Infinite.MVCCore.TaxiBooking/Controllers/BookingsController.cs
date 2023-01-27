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
    }
}
