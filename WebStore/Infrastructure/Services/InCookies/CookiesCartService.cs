using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Mapping;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Infrastructure.Services.InCookies
{
    public class CookiesCartService : ICartService
    {
        private readonly string _CartName;
        private readonly IProductData _ProductData;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        private Cart Cart
        {
            get
            {
                var context = _HttpContextAccessor.HttpContext;
                var cookies = context.Response.Cookies;
                var cart_cookie = context.Request.Cookies[_CartName];
                if (cart_cookie is null)
                {
                    var cart = new Cart();
                    cookies.Append(_CartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }

                ReplaceCookie(cookies, cart_cookie);
                return JsonConvert.DeserializeObject<Cart>(cart_cookie);
            }
            set => ReplaceCookie(_HttpContextAccessor.HttpContext.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        private void ReplaceCookie(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_CartName);
            cookies.Append(_CartName, cookie, new CookieOptions { Expires = DateTime.Now.AddDays(15) });
        }

        public CookiesCartService(IProductData ProductData, IHttpContextAccessor HttpContextAccessor)
        {
            _ProductData = ProductData;
            _HttpContextAccessor = HttpContextAccessor;

            var user = HttpContextAccessor.HttpContext.User;
            var user_name = user.Identity.IsAuthenticated ? user.Identity.Name : null;
            _CartName = $"WebStore.Cart[{user_name}]";
        }

        public void AddToCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                cart.Items.Add(new CartItem {ProductId = id, Quantity = 1});
            else
                item.Quantity++;

            Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null) return;
            if (item.Quantity > 0)
                item.Quantity--;
            if (item.Quantity == 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null) return;
            cart.Items.Remove(item);

            Cart = cart;
        }

        public void RemoveAll()
        {
            var cart = Cart;
            cart.Items.Clear();
            Cart = cart;
        }

        public CartViewModel TransformFromCart()
        {
            var cart_items = Cart.Items;
            var products = _ProductData
               .GetProducts(new ProductFilter
                {
                    Ids = cart_items.Select(item => item.ProductId).ToList()
                })
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
