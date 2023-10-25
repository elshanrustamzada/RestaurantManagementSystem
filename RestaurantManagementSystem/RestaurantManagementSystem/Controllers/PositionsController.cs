using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagementSystem.Controllers
{
    public class PositionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
