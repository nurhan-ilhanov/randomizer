using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Randomizer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Randomizer.Web.Data.Repositories;
using Sakura.AspNetCore;

namespace Randomizer.Web.Controllers
{
    [Authorize]
    public class ElementsController : Controller
    {
        private readonly IElementsRepository _repository;
        private readonly IElementListsRepository _listsRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly int _pageSize;

        public ElementsController(IElementsRepository repository, 
            IElementListsRepository listsRepository, 
            UserManager<ApplicationUser> userManager,
            int pageSize = 10)
        {
            _pageSize = pageSize;
            _repository = repository;
            _listsRepository = listsRepository;
            _userManager = userManager;
        }

        public IActionResult Index(int page = 1)
        {
            var elements = _repository.AllWhere(
                e => e.UserID == _userManager.GetUserId(User),
                e => e.User, e => e.ElementList)
                .OrderBy(e => e.Name)
                .AsNoTracking()
                .ToPagedList(_pageSize, page);

            return View(elements);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id.HasValue == false)
            {
                return NotFound();
            }

            var element = await _repository.GetSingle(m => m.ID == id, e => e.User);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        [HttpGet]
        public IActionResult Create(int? listID)
        {
            if (listID.HasValue == false)
            {
                return NotFound();
            }

            var list = _listsRepository.GetById(listID.Value);

            if (list == null)
            {
                return NotFound();
            }

            var element = new SimpleElement();

            return View(element);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int listID, SimpleElement model)
        {
            var list = await _listsRepository.GetSingle(m => m.ID == listID);

            if (list == null)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        model.UserID = _userManager.GetUserId(User);
                    }

                    model.ElementListID = listID;

                    _repository.Create(model);
                    await _repository.Save();

                    return RedirectToAction("Edit", "ElementLists", new { id = listID});
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

            var element = await _repository.GetSingle(m => m.ID == id);

            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id.HasValue == false)
            {
                return NotFound();
            }

            var elementToUpdate = await _repository.All().SingleOrDefaultAsync(s => s.ID == id);

            if (await TryUpdateModelAsync<SimpleElement>(
                elementToUpdate,
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

            return View(elementToUpdate);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id.HasValue == false)
            {
                return NotFound();
            }

            var element = await _repository.GetSingle(m => m.ID == id, e => e.User);

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
            var element = await _repository.GetSingle(m => m.ID == id);
            if (element == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                _repository.Delete(element);
                await _repository.Save();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
        }
    }
}