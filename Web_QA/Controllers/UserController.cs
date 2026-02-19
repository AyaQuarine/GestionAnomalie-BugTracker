using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GestionAnomalies.Repositories.Interfaces;
using GestionAnomalies.Entities;
using GestionAnomalies.Entities.Enums;

namespace GestionAnomalies.Controllers
{
    [Authorize(Roles = "Administrateur")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var users = await _userRepo.GetAllAsync();
            return View(users);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.DateCreation = DateTime.Now;
                await _userRepo.AddAsync(user);
                TempData["SuccessMessage"] = "Utilisateur créé avec succès.";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.DateModification = DateTime.Now;
                await _userRepo.UpdateAsync(user);
                TempData["SuccessMessage"] = "Utilisateur modifié avec succès.";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRepo.DeleteAsync(id);
            TempData["SuccessMessage"] = "Utilisateur supprimé avec succès.";
            return RedirectToAction(nameof(Index));
        }
    }
}
