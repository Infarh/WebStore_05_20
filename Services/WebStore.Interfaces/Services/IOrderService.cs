using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.DTO.Orders;

namespace WebStore.Interfaces.Services
{
    public interface IOrderService
    {
        IEnumerable<OrderDTO> GetUserOrders(string UserName);

        OrderDTO GetOrderById(int id);

        Task<OrderDTO> CreateOrderAsync(string UserName, CreateOrderModel OrderModel);
    }
}
