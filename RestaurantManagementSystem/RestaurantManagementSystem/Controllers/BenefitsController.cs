
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.DataAccessLayer;
using RestaurantManagementSystem.Helpers;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Controllers
{
    [Authorize(Roles="Member,Admin")]
    public class BenefitsController : Controller
    {
        private readonly AppDbContext _db;

        public BenefitsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Benefit> benefits =await _db.Benefits.OrderByDescending(x=>x.Id).ToListAsync();
            return View(benefits);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Benefit benefit)
        {
            if (benefit.Amount <= 0)
            {
                ModelState.AddModelError("Amount", "Məbləğ düzgün daxil edilməyib.");
                return View();
            }
            benefit.By = User.Identity.Name;
            Budget budget = await _db.Budgets.FirstOrDefaultAsync();
            budget.LastModifiedDescription = benefit.Description;
            budget.LastModifiedDate = DateTime.UtcNow.AddHours(4);
            budget.LastModifiedAmount = benefit.Amount;
            budget.LastModifiedBy = benefit.By;
            budget.TotalBudget += benefit.Amount;

            await _db.Benefits.AddAsync(benefit);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
