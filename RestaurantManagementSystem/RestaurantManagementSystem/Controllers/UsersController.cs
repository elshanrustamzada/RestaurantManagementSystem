
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Helpers;
using RestaurantManagementSystem.Models;
using RestaurantManagementSystem.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RestaurantManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersController(UserManager<AppUser> userManager,
                               RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            List<AppUser> dbUsers = await _userManager.Users.ToListAsync();
            List<UserVM> userVMs = new List<UserVM>();
            foreach (AppUser dbUser in dbUsers)
            {
                UserVM userVM = new UserVM
                {
                    Id = dbUser.Id,
                    Fullname = dbUser.Name + " " + dbUser.Surname,
                    Username = dbUser.UserName,
                    Email = dbUser.Email,
                    IsDeactive = dbUser.IsDeactive,
                    Role = (await _userManager.GetRolesAsync(dbUser))[0]
                };

                userVMs.Add(userVM);
            }
            return View(userVMs);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            ViewBag.Roles = new List<string>
            {
                 Roles.Admin.ToString(),
                 Roles.Member.ToString()
               };
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVM createVM, string role)
        {
            ViewBag.Roles = new List<string>
               {
                 Roles.Admin.ToString(),
                 Roles.Member.ToString()
               };
            AppUser newUser = new()
            {
                Name = createVM.Name,
                Email = createVM.Email,
                Surname = createVM.Surname,
                UserName = createVM.Username
            };
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, createVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(newUser, role);
            return RedirectToAction("Index");
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            if (!user.IsDeactive)
            {
                user.IsDeactive = true;
            }
            else
            {
                user.IsDeactive = false;
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        public async Task<IActionResult> Update(string id)
        {
            #region Get
            if (id == null)
            {
                return NotFound();
            }
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            UpdateVM dbUpdateVM = new()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Username = user.UserName,
                Role = (await _userManager.GetRolesAsync(user))[0],
            };
            ViewBag.Roles = new List<string>
            {
                Roles.Admin.ToString(),
                 Roles.Member.ToString()
               };
            #endregion

            return View(dbUpdateVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, UpdateVM updateVM, string role)
        {

            #region Get
            if (id == null)
            {
                return NotFound();
            }
            AppUser? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest();
            }
            UpdateVM dbUpdateVM = new()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Username = user.UserName,
                Role = (await _userManager.GetRolesAsync(user))[0],
            };
            ViewBag.Roles = new List<string>
               {
                 Roles.Admin.ToString(),
                 Roles.Member.ToString()
               };
            #endregion

            user.Name = updateVM.Name;
            user.Surname = updateVM.Surname;
            user.Email = updateVM.Email;
            user.UserName = updateVM.Username;


            if (dbUpdateVM.Role != role)
            {
                IdentityResult removeIdentityResult = await _userManager.RemoveFromRoleAsync(user, dbUpdateVM.Role);
                if (!removeIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in removeIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
                IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(user, role);
                if (!addIdentityResult.Succeeded)
                {
                    foreach (IdentityError error in addIdentityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View();
                }
            }


            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        #endregion

        #region ResetPassword
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, ResetPasswordVM resetPasswordVM)
        {
            if (id == null)
            {
                return NotFound();
            }

            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return BadRequest();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            return RedirectToAction("Index", "Dashboard");
        }
        #endregion

    }
}
