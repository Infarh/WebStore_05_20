using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class UserProfileController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Orders([FromServices] IOrderService OrderService) => 
            View(OrderService.GetUserOrders(User.Identity.Name)
               .Select(order => new UserOrderViewModel
                {
                    Id = order.Id,
                    Name = order.Name,
                    Address = order.Address,
                    Phone = order.Phone,
                    TotalSum = order.OrderItems.Sum(i => i.Price * i.Quantity)
                }));
    }
}