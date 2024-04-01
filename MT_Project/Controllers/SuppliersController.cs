using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MT_app.business.Services;
using MT_app.core.Models;

namespace MT_Project.Controllers
{
    [Authorize]

    public class SuppliersController : Controller
    {
        private readonly ISupplierService supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            this.supplierService = supplierService;
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
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(long id)
        {
            if (supplierService.Delete(id).IsCompleted)
            {
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}