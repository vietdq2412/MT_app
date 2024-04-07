using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MT_app.business.Services;
using MT_app.core.Models;

namespace MT_Project.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IMemoryCache cache;


        public CategoriesController(ICategoryService categoryService, IMemoryCache cache)
        {
            this.categoryService = categoryService;
            this.cache = cache;
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
            List<Category> categories = await categoryService.FindAll();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            cache.Set("categories", categories, cacheEntryOptions);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long id)
        {
            await categoryService.Delete((await categoryService.FindById(id))!);
            return RedirectToAction(nameof(Index));
        }
    }
}