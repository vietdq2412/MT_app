using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MT_app.business.Services;
using MT_app.core.Models;
using MT_app.core.ViewModel;

namespace MT_Project.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IProductService productService;
        private readonly IOrderService orderService;
        private readonly IIdentityService identityService;
        private readonly IOrderDetailService orderDetailService;
        private readonly IAppUserService appUserService;
        private readonly ICustomerService customerService;

        public OrderController(
            IOrderService _orderService,
            IOrderDetailService _orderDetailService,
            IProductService _productService,
            IIdentityService _identityService,
            IAppUserService appUserService,
            ICustomerService customerService)
        {
            this.orderService = _orderService;
            this.orderDetailService = _orderDetailService;
            this.productService = _productService;
            this.identityService = _identityService;
            this.appUserService = appUserService;
            this.customerService = customerService;
        }

        public async Task<ActionResult> Index(int? page)
        {
            string username = User.Identity.Name;
            List<Order> list =
                await orderService.FindOrdersByUsernameAndStatus(username, OrderStatus.Ordering.ToString());
            int pageNumber = page ?? 1;
            ViewData["Ordering"] = list;
            ViewData["Page"] = pageNumber;
            return View();
        }

        public async Task<ActionResult> History()
        {
            List<Order> list = await orderService.FindByStatus(OrderStatus.Pending.ToString());
            ViewData["PendingList"] = await orderService.FindByStatus(OrderStatus.Pending.ToString());
            ViewData["DeliveredList"] = await orderService.FindByStatus(OrderStatus.Delivered.ToString());
            ViewData["CancelledList"] = await orderService.FindByStatus(OrderStatus.Cancelled.ToString());
            return View();
        }

        public async Task<ActionResult> Cart()
        {
            List<OrderDetail> orderDetails = await orderDetailService.FindOrderingItems(User.Identity!.Name!);
            ViewData["orderDetails"] = orderDetails;
            return View();
        }

        [Authorize]
        public async Task<ActionResult> AddProductToOrder(long productId, int quantity)
        {
            Product product = productService.FindById(productId).Result!;
            List<Order> order = await orderService.FindOrdersByUsernameAndStatus(User.Identity.Name, OrderStatus.Ordering.ToString());
            OrderDetail orderDetail = new OrderDetail()
            {
                ProductId = productId,
                Product = product,
                Quantity = quantity,
                TotalPrice = product.Price * quantity
            };

            if (order.Any())
            {
                orderDetail.Order = order[0];
            }
            else
            {
                AppUser user = await appUserService.GetByUserLogin(User);
                Order newOrder = new Order()
                {
                    Name = user.FirstName + " " + user.LastName,
                    AppUser = user,
                    Status = OrderStatus.Ordering.ToString(),
                    TotalPrice = 0
                };
                orderDetail.Order = newOrder;
            }

            await orderDetailService.Save(orderDetail);
            return Redirect(nameof(Cart));
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(OrderViewModel orderViewModel)
        {
            Order order = (await orderService.FindById(orderViewModel.OrderId))!;
            Customer customer = (await customerService.FindById(orderViewModel.CustomerId))!;

            order.Customer = customer;
            order.Address = orderViewModel.Address;
            order.Note = orderViewModel.Note;

            order.Status = OrderStatus.Processing.ToString();

            await orderService.Update(order);
            return Redirect(nameof(Cart));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            AppUser user = await appUserService.GetByUserLogin(User);

            order.Status = OrderStatus.Ordering.ToString();
            order.AppUser = user;
            await orderService.Save(order);
            return Redirect(nameof(Index));
        }
    }
}