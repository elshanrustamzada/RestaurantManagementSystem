using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using RestaurantManagementSystem.DataAccessLayer;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Controllers
{
    [Authorize(Roles ="Member,Admin")]
    public class CostsController : Controller
    {
        private readonly AppDbContext _db;
        public CostsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            decimal take = 3;
            ViewBag.PageCount = Math.Ceiling((await _db.Costs.CountAsync() / take));
            List<Cost> costs = await _db.Costs.OrderByDescending(x => x.Id).ToListAsync();
            return View(costs);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cost cost)
        {
            if (cost.Amount <= 0)
            {
                ModelState.AddModelError("Amount", "Məbləğ düzgün daxil edilməyib.");
                return View();
            }
            cost.By = User.Identity.Name;
            Budget? budget = await _db.Budgets.FirstOrDefaultAsync();
            budget.LastModifiedDescription = cost.Description;
            budget.LastModifiedDate = DateTime.UtcNow.AddHours(4);
            budget.LastModifiedAmount = cost.Amount;
            budget.LastModifiedBy = cost.By;
            budget.TotalBudget -= cost.Amount;
            await _db.Costs.AddAsync(cost);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
