using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Orders;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;
using WebStore.Domain.ViewModels.Orders;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products.InSQL
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreDB _db;
        private readonly UserManager<User> _UserManager;

        public SqlOrderService(WebStoreDB db, UserManager<User> UserManager)
        {
            _db = db;
            _UserManager = UserManager;
        }

        public IEnumerable<OrderDTO> GetUserOrders(string UserName) => _db.Orders
           .Include(order => order.User)
           .Include(order => order.OrderItems)
           .Where(order => order.User.UserName == UserName)
           .AsEnumerable()
           .Select(OrderMapping.ToDTO);

        public OrderDTO GetOrderById(int id) => _db.Orders
           .Include(order => order.OrderItems)
           .FirstOrDefault(order => order.Id == id)
           .ToDTO();

        public async Task<OrderDTO> CreateOrderAsync(string UserName, CreateOrderModel OrderModel)
        {
            var user = await _UserManager.FindByNameAsync(UserName);

            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                var order = new Order
                {
                    Name = OrderModel.OrderViewModel.Name,
                    Address = OrderModel.OrderViewModel.Address,
                    Phone = OrderModel.OrderViewModel.Phone,
                    User = user,
                    Date = DateTime.Now
                };

                await _db.Orders.AddAsync(order);

                foreach (var item in OrderModel.OrderItems)
                {
                    var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == item.Id);
                    if(product is null)
                        throw new InvalidOperationException($"Товар с id:{item.Id} в базе данных на найден!");

                    var order_item = new OrderItem
                    {
                        Order = order,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        Product = product
                    };

                    await _db.OrderItems.AddAsync(order_item);
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return order.ToDTO();
            }
        }
    }
}
