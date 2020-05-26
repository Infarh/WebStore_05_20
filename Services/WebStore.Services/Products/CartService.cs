using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Products
{
    public class CartService : ICartService
    {
        private readonly IProductData _ProductData;
        private readonly ICartStore _CartStore;

        public CartService(IProductData ProductData, ICartStore CartStore)
        {
            _ProductData = ProductData;
            _CartStore = CartStore;
        }

        public void AddToCart(int id)
        {
            var cart = _CartStore.Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                cart.Items.Add(new CartItem {ProductId = id, Quantity = 1});
            else
                item.Quantity++;

            _CartStore.Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = _CartStore.Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null) return;
            if (item.Quantity > 0)
                item.Quantity--;
            if (item.Quantity == 0)
                cart.Items.Remove(item);

            _CartStore.Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = _CartStore.Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null) return;
            cart.Items.Remove(item);

            _CartStore.Cart = cart;
        }

        public void RemoveAll()
        {
            var cart = _CartStore.Cart;
            cart.Items.Clear();
            _CartStore.Cart = cart;
        }

        public CartViewModel TransformFromCart()
        {
            var cart_items = _CartStore.Cart.Items;
            var products = _ProductData
               .GetProducts(new ProductFilter
                {
                    Ids = cart_items.Select(item => item.ProductId).ToList()
                })
               .FromDTO()
               .ToView()
               .ToDictionary(p => p.Id);

            return new CartViewModel
            {
                Items = cart_items
                   .Where(item => products.ContainsKey(item.ProductId))
                   .ToDictionary(
                        item => products[item.ProductId],
                        item => item.Quantity
                    )
            };
        }
    }
}
