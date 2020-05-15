using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(IConfiguration Configuration) : base(Configuration, WebAPI.Orders) { }

        public IEnumerable<OrderDTO> GetUserOrders(string UserName) => Get<IEnumerable<OrderDTO>>($"{_ServiceAddress}/user/{UserName}");

        public OrderDTO GetOrderById(int id) => Get<OrderDTO>($"{_ServiceAddress}/{id}");

        public async Task<OrderDTO> CreateOrderAsync(string UserName, CreateOrderModel OrderModel)
        {
            var response = await PostAsync($"{_ServiceAddress}/{UserName}", OrderModel);
            return await response.Content.ReadAsAsync<OrderDTO>();
        }
    }
}
