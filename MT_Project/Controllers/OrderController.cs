﻿using Microsoft.AspNetCore.Mvc;
using MT_app.business.Services;
using MT_app.core.Models;

namespace MT_Project.Controllers
{
    public class OrderController : Controller
    {
        private readonly IProductService productService;
        private readonly IOrderService orderService;
        private readonly IOrderDetailService orderDetailService;

        public OrderController(
            IOrderService _orderService,
            IOrderDetailService _orderDetailService,
            IProductService productService)
        {
            this.orderService = _orderService;
            this.orderDetailService = _orderDetailService;
            this.productService = productService;
        }

        public async Task<ActionResult> Index()
        {
            List<Order> list= await orderService.FindByStatus(OrderStatus.Pending.ToString());
            ViewData["PendingList"] = await orderService.FindByStatus(OrderStatus.Pending.ToString());
            ViewData["AcceptedList"] = await orderService.FindByStatus(OrderStatus.Accepted.ToString());
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

        public async Task<ActionResult> AddProductToCart(long productId, int quantity)
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
    }
}