using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MT_app.business.Services;
using MT_app.core.Models;

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

        public OrderController(
            IOrderService _orderService,
            IOrderDetailService _orderDetailService,
            IProductService _productService,
            IIdentityService _identityService,
            IAppUserService appUserService)
        {
            this.orderService = _orderService;
            this.orderDetailService = _orderDetailService;
            this.productService = _productService;
            this.identityService = _identityService;
            this.appUserService = appUserService;
        }

        public async Task<ActionResult> Index()
        {
            string username = User.Identity.Name;
            List<Order> list = await orderService.FindOrderingOrdersByUsername(username);
            ViewData["Ordering"] = list;
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

        public ActionResult Cart()
        {
            List<OrderDetail> orderDetails = orderDetailService.FindItemsNotOrdered();
            ViewData["orderDetails"] = orderDetails;
            return View();
        }

        [Authorize]
        public async Task<ActionResult> AddProductToOrder(long productId, int quantity)
        {
            Product product = productService.FindById(productId).Result!;
            OrderDetail orderDetail = new OrderDetail()
            {
                ProductId = productId,
                Product = product,
                Quantity = quantity,
                TotalPrice = product.Price * quantity
            };
            await orderDetailService.Save(orderDetail);
            return Redirect(nameof(Cart));
        }

        public async Task<ActionResult> PlaceOrder()
        {
            Order order = new Order()
            {
                Name = "order",
                OrderDetails = orderDetailService.FindItemsNotOrdered(),
                Status = OrderStatus.Pending.ToString(),
            };
            order.TotalPrice = 0;
            foreach (var item in order.OrderDetails)
            {
                order.TotalPrice += item.TotalPrice;
            }

            await orderService.Save(order);
            return Redirect(nameof(Cart));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            AppUser user = await appUserService.GetByUserLogin(User);

            order.Status = OrderStatus.Ordering.ToString();
            order.AppUser = user;
           await orderService.Save(order);
            return Ok();
        }
    }
}