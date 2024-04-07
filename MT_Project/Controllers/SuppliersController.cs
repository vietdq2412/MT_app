using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MT_app.business.Services;
using MT_app.core.Models;

namespace MT_Project.Controllers
{
    [Authorize]

    public class SuppliersController : Controller
    {
        private readonly ISupplierService supplierService;
        private readonly IMemoryCache cache;


        public SuppliersController(ISupplierService supplierService, IMemoryCache cache)
        {
            this.supplierService = supplierService;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            List<Supplier> list = await supplierService.FindAll();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            await supplierService.Save(supplier);
            List<Supplier> suppliers = await supplierService.FindAll();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            cache.Set("suppliers", suppliers, cacheEntryOptions);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(long id)
        {
            await supplierService.Delete((await supplierService.FindById(id))!);
            return RedirectToAction(nameof(Index));
        }
    }
}