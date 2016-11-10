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

namespace Randomizer.Web.Controllers
{
    [Authorize]
    public class ElementListsController : Controller
    {
        private readonly RandomizerContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ElementListsController(RandomizerContext context, UserManager<ApplicationUser> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var lists = _db.ElementLists.OrderBy(e => e.Name)
                .AsNoTracking()
                .ToList();

            return View(lists);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _db.ElementLists
                .Include(l => l.Elements)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

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
            this.PopulateElementsInList(list);

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ElementList model, string[] selectedElements)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.UpdateListElemens(selectedElements, model);
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

            var list = await _db.ElementLists
                .Include(l => l.Elements)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (list == null)
            {
                return NotFound();
            }

            this.PopulateElementsInList(list);
            return View(list);
        }

        private void PopulateElementsInList(ElementList list)
        {
            var allElements = _db.Elements;
            var listElements = new List<int>(list.Elements.Select(e => e.ID));
            var viewModel = new List<ListElementViewModel>();

            foreach (var element in allElements)
            {
                viewModel.Add(new ListElementViewModel
                {
                    ElementID = element.ID,
                    ElementName = element.Name,
                    InList = listElements.Contains(element.ID)
                });
            }
            ViewData["Elements"] = viewModel;

        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id, string[] selectedElements)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listToUpdate = await _db.ElementLists
                .Include(l => l.Elements)
                .SingleOrDefaultAsync(s => s.ID == id);

            if (await TryUpdateModelAsync<ElementList>(
                listToUpdate,
                "",
                s => s.Name, s => s.Description))
            {
                this.UpdateListElemens(selectedElements, listToUpdate);
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

            return View(listToUpdate);
        }

        private void UpdateListElemens(string[] selectedElements, ElementList list)
        {
            if (selectedElements == null)
            {
                list.Elements = new List<SimpleElement>();
                return;
            }

            var selectedElementsL = new List<string>(selectedElements);
            var listElements = new List<int>
                (list.Elements.Select(e => e.ID));

            foreach (var element in _db.Elements)
            {
                if (selectedElementsL.Contains(element.ID.ToString()))
                {
                    if (!listElements.Contains(element.ID))
                    {
                        list.Elements.Add(element);
                    }
                }

                else
                {

                    if (listElements.Contains(element.ID))
                    {
                        SimpleElement elementToRemove = list.Elements.SingleOrDefault(i => i.ID == element.ID);
                        list.Elements.Remove(elementToRemove);
                    }
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _db.ElementLists
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

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
            var list = await _db.ElementLists
                .Include(l => l.Elements)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (list == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                _db.ElementLists.Remove(list);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetRandomElement(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _db.ElementLists
                .Include(l => l.Elements)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (list == null)
            {
                return NotFound();
            }

            var randomizerEngine = new RandomizerEngine();
            var randomElement = await randomizerEngine.GetElement(list.Elements.AsQueryable());
            
            return Json(new { ans = randomElement.Name});
        }

        [HttpPost]
        public async Task<IActionResult> GetRandomElements(int? id, int count)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _db.ElementLists
                .Include(l => l.Elements)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (list == null)
            {
                return NotFound();
            }

            var randomizerEngine = new RandomizerEngine();
            var randomElements = await randomizerEngine.GetElements(list.Elements.AsQueryable(), count);

            var names = "";
            foreach (var element in randomElements)
            {
                 names += element.Name + "\n";
            }
            return Json(new { ans = names });
        }
    }
}