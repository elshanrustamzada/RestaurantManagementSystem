using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.DataAccessLayer;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class PositionsController : Controller
    {
        private readonly AppDbContext _db;
        public PositionsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Position> positions = await _db.Positions.ToListAsync();
            return View(positions);
        }

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            #region Is Exist
            bool isExist = await _db.Positions.AnyAsync(x => x.Name == position.Name);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu vəzifə artıq mövcuddur!");
                return View();
            }
            #endregion

            await _db.Positions.AddAsync(position);
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
            Position? dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPosition == null)
            {
                return BadRequest();
            }
            return View(dbPosition);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Position position)
        {
            if (id == null)
            {
                return NotFound();
            }
            Position? dbPosition = await _db.Positions.FirstOrDefaultAsync(x => x.Id == id);
            if (dbPosition == null)
            {
                return BadRequest();
            }

            #region Is Exist
            bool isExist = await _db.Positions.AnyAsync(x => x.Name == position.Name && x.Id != id);
            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu vəzifə artıq mövcuddur!");
                return View();
            }
            #endregion

            dbPosition.Name = position.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

    }
}
