using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Randomizer.Web.Data;
using Randomizer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Randomizer.Web.Models;
using Randomizer.Core;
using Microsoft.AspNetCore.Authorization;
using Randomizer.Web.Data.Repositories;
using Sakura.AspNetCore;
using Microsoft.Extensions.Localization;

namespace Randomizer.Web.Controllers
{
    [Authorize]
    public class ElementListsController : Controller
    {
        private readonly IElementListsRepository _repository;
        private readonly IElementsRepository _elementsRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly int _pageSize;

        public ElementListsController(IElementListsRepository repository,
            IElementsRepository elementsRepo,
            UserManager<ApplicationUser> userManager,
            int pageSize = 10)
        {
            _pageSize = pageSize;
            _repository = repository;
            _elementsRepo = elementsRepo;
            _userManager = userManager;
        }

        public IActionResult Index(int page = 1)
        {
            var lists = _repository.AllWhere(l => l.UserID == _userManager.GetUserId(User))
                .OrderBy(e => e.Name)
                .AsNoTracking()
                .ToPagedList(_pageSize, page);

            return View(lists);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id.HasValue == false)
            {
                return NotFound();
            }

            var list = await _repository.GetSingle(m => m.ID == id,
                l => l.Elements);

            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var list = new ElementList();

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ElementList model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        model.UserID = _userManager.GetUserId(User);
                    }

                    _repository.Create(model);
                    await _repository.Save();

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
            if (id.HasValue == false)
            {
                return NotFound();
            }

            var list = await _repository.GetSingle(m => m.ID == id,
                l => l.Elements);

            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id.HasValue == false)
            {
                return NotFound();
            }

            var listToUpdate = _repository.AllWhere(l => l.ID == id)
                .Include(l => l.Elements)
                    .ThenInclude(e => e.User)
                .SingleOrDefault();

            if (await TryUpdateModelAsync<ElementList>(
                listToUpdate,
                "",
                s => s.Name, s => s.Description))
            {

                try
                {
                    await _repository.Save();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }

            return View(listToUpdate);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id.HasValue == false)
            {
                return NotFound();
            }

            var list = await _repository.GetSingle(m => m.ID == id);

            if (list == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(list);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var list = await _repository.GetSingle(m => m.ID == id,
                l => l.Elements);

            if (list == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                _repository.Delete(list);
                await _repository.Save();

                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }

        /// <summary>
        /// Takes an item from a list.
        /// </summary>
        /// <param name="id">The ID of the list</param>
        /// <returns>The name of the item</returns>
        [HttpPost]
        public async Task<IActionResult> DrawItem(int? id)
        {
            if (id.HasValue == false)
            {
                return NotFound();
            }

            var list = await _repository.GetSingle(m => m.ID == id,
                l => l.Elements);

            if (list == null)
            {
                return NotFound();
            }

            var randomizerEngine = new RandomizerEngine();
            string elementName = "";

            try
            {
                var randomElement = await randomizerEngine.GetElement(list.Elements.AsQueryable());
                elementName = randomElement.Name;
            }
            catch (ArgumentOutOfRangeException)
            {
                return Json(new { ans = "There are not enough items in your list!" });
            }

            return Json(new { ans = elementName });
        }

        /// <summary>
        /// Takes N number of items from a list.
        /// </summary>
        /// <param name="id">ID of the list</param>
        /// <param name="count">The number of items to take</param>
        /// <returns>A string with names of the items</returns>
        [HttpPost]
        public async Task<IActionResult> DrawItems(int? id, int count)
        {
            if (id.HasValue == false)
            {
                return NotFound();
            }

            var list = await _repository.GetSingle(m => m.ID == id,
                 l => l.Elements);

            if (list == null)
            {
                return NotFound();
            }

            var randomizerEngine = new RandomizerEngine();
            var names = "";

            try
            {
                var randomElements = await randomizerEngine.GetElements(list.Elements.AsQueryable(), count);

                foreach (var element in randomElements)
                {
                    names += element.Name + "<br/>";
                }
            }
            catch (ArgumentOutOfRangeException)
            {

                return Json(new { ans = "There are not enough items in your list!" });
            }

            return Json(new { ans = names });
        }
    }
}