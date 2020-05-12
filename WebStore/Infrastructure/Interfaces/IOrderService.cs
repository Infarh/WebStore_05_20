using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Orders;
using WebStore.ViewModels;
using WebStore.ViewModels.Orders;

namespace WebStore.Infrastructure.Interfaces
{
    public interface IOrderService
    {
        IEnumerable<Order> GetUserOrders(string UserName);

        Order GetOrderById(int id);

        Task<Order> CreateOrderAsync(string UserName, CartViewModel Cart, OrderViewModel OrderModel);
    }
}
