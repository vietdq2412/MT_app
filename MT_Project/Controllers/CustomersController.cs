using Microsoft.AspNetCore.Mvc;
using MT_app.business.Services;
using MT_app.core.Models;

namespace MT_Project.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(
            ICustomerService customerService)
        {
            this._customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            List<Customer> list = await _customerService.FindAll();
            return View(list);
        }

        [HttpGet]
        public JsonResult GetAllCustomer()
        {
            return Json(_customerService.FindAll());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            await _customerService.Save(customer);
            return RedirectToAction(nameof(Index));
        }
    }
}