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
            var message = TempData["Message"] as string;
            var error = TempData["Error"] as string;
            ViewData["Message"] = message;
            ViewData["Error"] = error;
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
            if (!_customerService.CheckDuplicatePhoneNumber(customer.PhoneNumber))
            {
                await _customerService.Save(customer);
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Phone number duplicated!!");
            TempData["Error"] = "Phone number duplicated!!";
            return RedirectToAction(nameof(Index));

        }
    }
}