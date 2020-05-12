using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Orders;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;
using WebStore.ViewModels.Orders;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _CartService;

        public CartController(ICartService CartService) => _CartService = CartService;

        public IActionResult Details() => View(new CartOrderViewModel
        {
            CartViewModel = _CartService.TransformFromCart(),
            OrderViewModel = new OrderViewModel()
        });

        public IActionResult AddToCart(int id)
        {
            _CartService.AddToCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult DecrementFromCart(int id)
        {
            _CartService.DecrementFromCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult RemoveFromCart(int id)
        {
            _CartService.RemoveFromCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult RemoveAll()
        {
            _CartService.RemoveAll();
            return RedirectToAction(nameof(Details));
        }

        public async Task<IActionResult> CheckOut(OrderViewModel Model, [FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Details), new CartOrderViewModel
                {
                    CartViewModel = _CartService.TransformFromCart(),
                    OrderViewModel = Model
                });

            var order = await OrderService.CreateOrderAsync(User.Identity.Name, _CartService.TransformFromCart(), Model);

            _CartService.RemoveAll();

            return RedirectToAction(nameof(OrderConfirmed), new { id = order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}