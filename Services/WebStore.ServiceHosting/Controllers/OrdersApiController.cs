using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain;
using WebStore.Domain.DTO.Orders;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>Контроллер заказов</summary>
    [Route(WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrderService
    {
        private readonly IOrderService _OrderService;
        private readonly ILogger<OrdersApiController> _Logger;

        public OrdersApiController(IOrderService OrderService, ILogger<OrdersApiController> Logger)
        {
            _OrderService = OrderService;
            _Logger = Logger;
        }

        /// <summary>Получить все заказы пользователя</summary>
        /// <param name="UserName">Имя пользователя</param>
        /// <returns>Возвращает список заказов указанного пользователя</returns>
        [HttpGet("user/{UserName}")]
        public IEnumerable<OrderDTO> GetUserOrders(string UserName) => _OrderService.GetUserOrders(UserName);

        /// <summary>Получить заказ по его идентификатору</summary>
        /// <param name="id">Идентификационный номер заказа</param>
        /// <returns>Заказ с указанным идентификатором, либо пустая ссылка, если заказ не найден</returns>
        [HttpGet("{id}")]
        public OrderDTO GetOrderById(int id) => _OrderService.GetOrderById(id);

        /// <summary>Создание нового заказа для указанного пользователя</summary>
        /// <param name="UserName">Имя пользователя для которого оформляется заказ</param>
        /// <param name="OrderModel">Структура формируемого заказа</param>
        /// <returns>Структура сформированного заказа</returns>
        [HttpPost("{UserName}")]
        public async Task<OrderDTO> CreateOrderAsync(string UserName, /*[FromBody] */CreateOrderModel OrderModel)
        {
            if(string.IsNullOrEmpty(UserName))
                throw new ArgumentException("Не указано имя пользователя");

            if(string.IsNullOrEmpty(OrderModel.OrderViewModel.Address))
                throw new ArgumentException("Не указан адрес доставки");

            
            _Logger.LogInformation("Создаётся заказ для пользователя {0}", UserName);

            var order = await _OrderService.CreateOrderAsync(UserName, OrderModel);

            _Logger.LogInformation("Заказ для пользователя {0} создан успешно", UserName);

            return order;
        }
    }
}