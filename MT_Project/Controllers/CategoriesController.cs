using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MT_app.business.Services;
using MT_app.core.Models;

namespace MT_Project.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            List<Category> list = await categoryService.FindAll();
            return View(list);
        }

        public async Task<IActionResult> Create(string catName)
        {
            Category category = new Category
            {
                Name = catName
            };
            await categoryService.Save(category);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(long id)
        {
            if (categoryService.Delete(id).IsCompleted)
            {
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}