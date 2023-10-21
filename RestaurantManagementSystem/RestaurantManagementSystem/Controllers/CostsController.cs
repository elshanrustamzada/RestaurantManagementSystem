using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RestaurantManagementSystem.DataAccessLayer;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class CostsController : Controller
    {
        private readonly AppDbContext _db;
        public CostsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Cost> costs = await _db.Costs.OrderByDescending(x => x.Id).ToListAsync();
            return View(costs);
        }
    }
}
