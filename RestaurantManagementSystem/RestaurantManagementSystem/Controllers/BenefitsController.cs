
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
    }
}
