using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.DataAccessLayer;
using RestaurantManagementSystem.Helpers;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class EmployersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public EmployersController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Employer> employers = await _db.Employers.OrderByDescending(x => x.Id).ToListAsync();
            return View(employers);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employer employer)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();

            #region Save Image
            if (employer.Photo == null)
            {
                ModelState.AddModelError("Photo", "please select photo");
                return View();
            }
            if (!employer.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "please select image type");
                return View();
            }
            if (employer.Photo.IsOrder1Mb())
            {
                ModelState.AddModelError("Photo", "Max 1Mb");
                return View();
            }
            string folder = Path.Combine(_env.WebRootPath, "images");
            employer.Image = await employer.Photo.SaveFileAsync(folder);
            #endregion

            #region Is Exist
            bool isExist = await _db.Employers.AnyAsync(x => x.Name == employer.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This employer is already exist");
                return View();
            }
            #endregion

            await _db.Employers.AddAsync(employer);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
