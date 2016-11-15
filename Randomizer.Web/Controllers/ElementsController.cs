using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Randomizer.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Randomizer.Web.Data.Repositories;

namespace Randomizer.Web.Controllers
{
    [Authorize]
    public class ElementsController : Controller
    {
        private readonly IElementsRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ElementsController(IElementsRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var elements = _repository.AllWhere(
                e => e.UserID == _userManager.GetUserId(User),
                e => e.User)
                .OrderBy(e => e.Name)
                .AsNoTracking()
                .ToList();

            return View(elements);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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
            if (id == null)
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
            if (id == null)
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
            if (id == null)
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