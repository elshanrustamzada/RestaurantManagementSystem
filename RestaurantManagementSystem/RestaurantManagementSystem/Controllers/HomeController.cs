using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Models;
using System.Diagnostics;

namespace RestaurantManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
       

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

       
    }
}