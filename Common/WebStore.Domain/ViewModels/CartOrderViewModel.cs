using WebStore.Domain.ViewModels.Orders;

namespace WebStore.Domain.ViewModels
{
    public class CartOrderViewModel
    {
        public CartViewModel CartViewModel { get; set; }

        public OrderViewModel OrderViewModel { get; set; }
    }
}
