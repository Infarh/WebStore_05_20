using Microsoft.AspNetCore.Mvc;

namespace WebStore.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}
