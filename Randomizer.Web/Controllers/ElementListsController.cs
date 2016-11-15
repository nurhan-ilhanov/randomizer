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

namespace Randomizer.Web.Controllers
{
    [Authorize]
    public class ElementListsController : Controller
    {
        private readonly IElementListsRepository _repository;
        private readonly IElementsRepository _elementsRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public ElementListsController(IElementListsRepository repository, IElementsRepository elementsRepo, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _elementsRepo = elementsRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var lists = _repository.AllWhere(l => l.UserID == _userManager.GetUserId(User))
                .OrderBy(e => e.Name)
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
            //this.PopulateElementsInList(list);

            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ElementList model, string[] selectedElements)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        model.UserID = _userManager.GetUserId(User);
                    }

                    //this.UpdateListElements(selectedElements, model);

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

            this.PopulateElementsInList(model);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _repository.GetSingle(m => m.ID == id,
                l => l.Elements);

            if (list == null)
            {
                return NotFound();
            }

            this.PopulateElementsInList(list);
            return View(list);
        }

        private void PopulateElementsInList(ElementList list)
        {

            var allElements = _elementsRepo.All().AsNoTracking();
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

            var listToUpdate = await _repository.GetSingle(s => s.ID == id, l => l.Elements);

            if (await TryUpdateModelAsync<ElementList>(
                listToUpdate,
                "",
                s => s.Name, s => s.Description))
            {
                this.UpdateListElements(selectedElements, listToUpdate);
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

            this.PopulateElementsInList(listToUpdate);
            return View(listToUpdate);
        }

        private void UpdateListElements(string[] selectedElements, ElementList list)
        {
            if (selectedElements == null)
            {
                list.Elements = new List<SimpleElement>();
                return;
            }

            var selectedElementsL = new List<string>(selectedElements);
            var listElements = new List<int>
                (list.Elements.Select(e => e.ID));


            foreach (var element in _elementsRepo.All())
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

        public async Task<IActionResult> AddItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _repository.GetSingle(m => m.ID == id);

            if (list == null)
            {
                return NotFound();
            }

            list.Elements.Add(new SimpleElement());

            return View(list);
        }

        [HttpPost]
        [ActionName("AddItem")]
        public async Task<IActionResult> AddItemPost(int? id, ElementList model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listToUpdate = await _repository.GetSingle(s => s.ID == id, l => l.Elements);

            try
            {

                if (User.Identity.IsAuthenticated)
                {
                    model.UserID = _userManager.GetUserId(User);
                }

                listToUpdate.Elements.Add(model.Elements.FirstOrDefault());

                await _repository.Save();
                return RedirectToAction("Index");

            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }

            return View(listToUpdate);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
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

        [HttpPost]
        public async Task<IActionResult> DrawItem(int? id)
        {
            if (id == null)
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

        [HttpPost]
        public async Task<IActionResult> DrawItems(int? id, int count)
        {
            if (id == null)
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
                    names += element.Name + "\n";
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