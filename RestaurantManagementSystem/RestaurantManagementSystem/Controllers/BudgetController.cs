using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.DataAccessLayer;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Controllers
{
    [Authorize(Roles ="Member,Admin")]
    public class BudgetController : Controller
    {
        private readonly AppDbContext _db;
        public BudgetController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            Budget? budget = await _db.Budgets.FirstOrDefaultAsync();
            return View(budget);
        }
    }
}
