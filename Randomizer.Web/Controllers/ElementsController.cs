using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Randomizer.Web.Data;
using Microsoft.EntityFrameworkCore;
using Randomizer.Model;
using Microsoft.AspNetCore.Identity;

namespace Randomizer.Web.Controllers
{
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
                .ToList();

            return View(elements);
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
                        var user = await _userManager.GetUserAsync(User);
                        model.UserID = user.Id;
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
        public async Task<IActionResult> EditPost(int? id, SimpleElement model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(model);
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
            return View(model);
        }
    }
}