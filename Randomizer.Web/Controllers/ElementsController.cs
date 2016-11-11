using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Randomizer.Web.Data;
using Microsoft.EntityFrameworkCore;
using Randomizer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Randomizer.Web.Controllers
{
    [Authorize]
    public class ElementsController : Controller
    {
        private readonly RandomizerContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ElementsController(RandomizerContext context, UserManager<ApplicationUser> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var elements = _db.Elements.OrderBy(e => e.Name)
                .Include(e => e.User)
                .AsNoTracking()
                .Where(e => e.UserID == _userManager.GetUserId(User))
                .ToList();

            return View(elements);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _db.Elements
                .Include(e => e.User)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var element = new SimpleElement();

            return View(element);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SimpleElement model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        model.UserID =  _userManager.GetUserId(User);
                    }

                    _db.Add(model);
                    await _db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _db.Elements
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elementToUpdate = await _db.Elements.SingleOrDefaultAsync(s => s.ID == id);

            if (await TryUpdateModelAsync<SimpleElement>(
                elementToUpdate,
                "",
                s => s.Name, s => s.Description))
            {
                try
                {
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }

            return View(elementToUpdate);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _db.Elements
                .Include(e => e.User)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (element == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(element);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var element = await _db.Elements
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (element == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                _db.Elements.Remove(element);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }
    }
}