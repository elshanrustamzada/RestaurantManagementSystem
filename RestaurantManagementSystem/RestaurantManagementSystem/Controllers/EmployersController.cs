using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
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
            List<Employer> employers = await _db.Employers.Include(x => x.Position).OrderByDescending(x => x.Id).ToListAsync();
            return View(employers);
        }

        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employer employer, int positionId)
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

            employer.PositionId = positionId;
            await _db.Employers.AddAsync(employer);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employer? dbEmployer = await _db.Employers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEmployer == null)
            {
                return BadRequest();
            }
            ViewBag.Positions = await _db.Positions.ToListAsync();
            return View(dbEmployer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Employer employer, int positionId)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employer? dbEmployer = await _db.Employers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEmployer == null)
            {
                return BadRequest();
            }
            ViewBag.Positions = await _db.Positions.ToListAsync();

            #region Save Image
            if (employer.Photo != null)
            {
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
                dbEmployer.Image = await employer.Photo.SaveFileAsync(folder);
            }

            #endregion

            #region Is Exist
            bool isExist = await _db.Employers.AnyAsync(x => x.Name == employer.Name && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "This employer is already exist");
                return View();
            }
            #endregion

            dbEmployer.Name = employer.Name;
            dbEmployer.Salary = employer.Salary;
            dbEmployer.Birthday = employer.Birthday;
            dbEmployer.PhoneNumber = employer.PhoneNumber;
            dbEmployer.PositionId = positionId;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employer? dbEmployer = await _db.Employers.Include(x => x.Position).FirstOrDefaultAsync(x=>x.Id==id);
            if (dbEmployer == null)
            {
                return BadRequest();
            }
            return View(dbEmployer);
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employer? dbEmployer = await _db.Employers.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEmployer == null)
            {
                return BadRequest();
            }

            if (dbEmployer.IsDeactive)
            {
                dbEmployer.IsDeactive = false;
            }
            else
            {
                dbEmployer.IsDeactive = true;
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        } 
        #endregion


    }


}
