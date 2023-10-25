using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.DataAccessLayer;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class EmployersController : Controller
    {
        private readonly AppDbContext _db;

        public EmployersController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Employer> employers = await _db.Employers.OrderByDescending(x => x.Id).ToListAsync();
            return View(employers);
        }
    }
}
